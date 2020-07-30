<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MyShipping.aspx.cs" Inherits="MyAccount_MySettings_MyShipping" Title="Shipping Information" %>

<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<style type ="text/css">
        .form_popup_box span.error
        {
        	padding:0px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $().ready(function() {

            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                },
                messages: {

            }
        }); // form validate

        // validate on click of shipping
        $("#<%=lnkBtnAddAnotherShiping.ClientID %>").click(function() {


            $("#ctl00_ContentPlaceHolder1_TxtShippingCompany").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "name") }
                    });

            $("#ctl00_ContentPlaceHolder1_TxtShippingName").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "name") }
                    });

            $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "title") }
                    });



//            $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").rules("add",
//                    {
//                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "email") }
//                    });


            $("#ctl00_ContentPlaceHolder1_TxtShippingZip").rules("add",
                    {
                        required: true, alphanumeric: true, validzipcode: GetCountryCode($("#ctl00_ContentPlaceHolder1_DrpShipingCoutry :selected").text()), messages: { required: replaceMessageString(objValMsg, "Required", "zip") }

                    });
            $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "mobile no") }

                    });
            $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").rules("add",
                    {
                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "telephone no") }

                    });


            $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "country") }
                    });

            $("#ctl00_ContentPlaceHolder1_DrpShipingState").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "state") }
                    });

            $("#ctl00_ContentPlaceHolder1_DrpShippingCity").rules("add",
                    {
                        NotequalTo: "0", messages: { NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "city") }
                    });

//            $("#ctl00_ContentPlaceHolder1_txtCity").rules("add",
//                    {
//                        required: true, messages: { required: replaceMessageString(objValMsg, "Required", "city name") }

//                    });

            $("#ctl00_ContentPlaceHolder1_TxtShippingCompany").valid();
            $("#ctl00_ContentPlaceHolder1_TxtShippingName").valid();
            $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").valid();
//            $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").valid();
            $("#ctl00_ContentPlaceHolder1_TxtShippingZip").valid();
            $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").valid();
            $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").valid();
            $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").valid();
            $("#ctl00_ContentPlaceHolder1_DrpShipingState").valid();
            $("#ctl00_ContentPlaceHolder1_DrpShippingCity").valid();
//            $("#ctl00_ContentPlaceHolder1_txtCity").valid();

            if ($("#ctl00_ContentPlaceHolder1_TxtShippingCompany").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingName").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").valid() == '0'
//                        || $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingZip").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShipingState").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShippingCity").valid() == '0'
                        || $("#ctl00_ContentPlaceHolder1_DrpShippingCity").valid() == '-Other-'
//                        || $("#ctl00_ContentPlaceHolder1_txtCity").valid() == '0'
                    ) {
                RemoveShippingRules();
                return false;
            }
            else {
                RemoveShippingRules();
                return true;
            }
        });
            $(".collapsibleContainer").collapsiblePanel();
            $(".collapsibleContainerContent").hide();
    }); //get
    function RemoveShippingRules() {
        //remove rules for shipping
        $("#ctl00_ContentPlaceHolder1_TxtShippingCompany").rules("remove");
        $("#ctl00_ContentPlaceHolder1_TxtShippingName").rules("remove");
        $("#ctl00_ContentPlaceHolder1_TxtShipingTitle").rules("remove");
//        $("#ctl00_ContentPlaceHolder1_TxtShippingEmail").rules("remove");
        $("#ctl00_ContentPlaceHolder1_TxtShippingZip").rules("remove");
        $("#ctl00_ContentPlaceHolder1_TxtShippingMobile").rules("remove");
        $("#ctl00_ContentPlaceHolder1_TxtShppingTelephone").rules("remove");
        $("#ctl00_ContentPlaceHolder1_DrpShipingCoutry").rules("remove");
        $("#ctl00_ContentPlaceHolder1_DrpShipingState").rules("remove");
        $("#ctl00_ContentPlaceHolder1_DrpShippingCity").rules("remove");
//        $("#ctl00_ContentPlaceHolder1_txtCity").rules("remove");
    }

    //set department dropdown value add by mayur on 26-jan-2012
    $('#Department option').each(function(i) {
        if ($("#ctl00_ContentPlaceHolder1_hdnDepartment").val() == $(this).val()) {
            $(this).attr("selected", "selected");
            $("#Department").msDropDown({ mainCSS: 'dd2' });
        }

    });

    $('#Department').change(function() {

        $("#ctl00_ContentPlaceHolder1_hdnDepartment").val($(this).val());
        return $('#ctl00_ContentPlaceHolder1_hdnDepartment').valid();
    });

    //end add by mayur on 26-jan-2012

    $(window).scroll(function() {
        $("#ctl00_ContentPlaceHolder1_hdnScrollY").val($(window).scrollTop());
    });

    $(window).scrollTop($("#ctl00_ContentPlaceHolder1_hdnScrollY").val());

});           //ready
function pageLoad(sender, args) {
    {

        assigndesign();
    }
}
function DeleteConfirmation() {
    if (confirm("Are you sure, you want to delete selected records ?") == true)
        return true;
    else
        return false;
}
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {

                assigndesign();
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="box_pad">
        <asp:HiddenField ID="hdnScrollY" runat="server" Value="0" />
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <div class="spacer10">
        </div>
        <div>
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
                                            <span class="input_label">Company</span>
                                            <asp:TextBox ID="TxtShippingCompany" runat="server" CssClass="w_label" TabIndex="1"></asp:TextBox>
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
                                            <span class="input_label">First Name</span>
                                            <asp:TextBox ID="TxtShippingName" runat="server" CssClass="w_label" TabIndex="4"></asp:TextBox>
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
                                            <span class="input_label">Address 2</span>
                                            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="35" TabIndex="7"></asp:TextBox>
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
                                            <span class="input_label">State</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="DrpShipingState" runat="server" OnSelectedIndexChanged="DrpShipingState_SelectedIndexChanged"
                                                    AutoPostBack="true" TabIndex="10">
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
                                            <span class="input_label">Zip</span>
                                            <asp:TextBox ID="TxtShippingZip" runat="server" CssClass="w_label" TabIndex="13"></asp:TextBox>
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
                                            <span class="input_label">Email</span>
                                            <asp:TextBox ID="TxtShippingEmail" runat="server" CssClass="w_label" TabIndex="17"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>--%>
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
                                            <span class="input_label">Address Name</span>
                                            <asp:TextBox ID="TxtShipingTitle" runat="server" CssClass="w_label" TabIndex="2"></asp:TextBox>
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
                                            <span class="input_label">Last Name</span>
                                            <asp:TextBox ID="TxtShipingFAX" runat="server" CssClass="w_label" TabIndex="5"></asp:TextBox>
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
                                            <span class="input_label">Suite/Apt.</span>
                                            <asp:TextBox ID="txtStreet" runat="server" MaxLength="35" TabIndex="8"></asp:TextBox>
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
                                                <asp:DropDownList ID="DrpShippingCity" AutoPostBack="true" runat="server" TabIndex="11"
                                                    OnSelectedIndexChanged="DrpShippingCity_SelectedIndexChanged">
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
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="w_label" TabIndex="11"></asp:TextBox>
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
                                            <asp:TextBox ID="TxtShppingTelephone" runat="server" CssClass="w_label" TabIndex="14"></asp:TextBox>
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
                                            <input id="hdnDepartment" type="hidden" value="0" runat="server" />
                                            <label class="dropimg_width">
                                                <span class="form_box status_select label-sel-small">
                                                    <select tabindex="3" id="Department" name="Department">
                                                        <option value="0">-select department-</option>
                                                        <%
                                                            Incentex.DAL.SqlRepository.CompanyStoreRepository objCompanyStoreRepository = new Incentex.DAL.SqlRepository.CompanyStoreRepository();
                                                            System.Int64 storeID = objCompanyStoreRepository.GetStoreIDByCompanyId((int)IncentexGlobal.CurrentMember.CompanyId);
                                                            System.Collections.Generic.List<Incentex.DAL.GetStoreDepartmentsResult> lstDepartments = objCompanyStoreRepository.GetStoreDepartments(storeID).Where(le => le.Existing == 1).OrderBy(le => le.Department).ToList();

                                                            foreach (Incentex.DAL.GetStoreDepartmentsResult Department in lstDepartments)
                                                            {
                                                                string path = "../../admin/Incentex_Used_Icons/" + Department.sLookupIcon.ToString();
                                                               
                                                                
                                                        %>
                                                        <option value="<%=Department.DepartmentID%>" title="<%=path%>">
                                                            <%=Department.Department.ToString()%></option>
                                                        <%
                                                            } %>
                                                    </select>
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
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Address 1</span>
                                            <asp:TextBox ID="TxtShipingAddress" runat="server" MaxLength="35" TabIndex="6"></asp:TextBox>
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
                                                <asp:DropDownList ID="DrpShipingCoutry" runat="server" CssClass="w_label" OnSelectedIndexChanged="DrpShipingCoutry_SelectedIndexChanged"
                                                    AutoPostBack="true" TabIndex="9">
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">County</span>
                                            <asp:TextBox ID="txtCounty" runat="server" CssClass="w_label" TabIndex="12"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Mobile</span>
                                            <asp:TextBox ID="TxtShippingMobile" runat="server" CssClass="w_label" TabIndex="15"></asp:TextBox>
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
                                            <span class="input_label">Airport</span>
                                            <asp:TextBox ID="txtAirport" runat="server" CssClass="w_label" TabIndex="18"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>                          
                           
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td class="formtd">
                        &nbsp;
                    </td>
                    <td style="text-align: right; white-space: nowrap;">
                       <%-- <asp:LinkButton ID="lnkBtnAddAnotherShiping" class="grey2_btn" runat="server" OnClick="lnkBtnAddAnotherShiping_Click"
                            TabIndex="19"><span>Save & Add New Shipping Address</span></asp:LinkButton>--%>
                             <asp:LinkButton ID="lnkBtnAddAnotherShiping" class="gredient_btnMainPage" title="Shipping Information"
                            runat="server" onclick="lnkBtnAddAnotherShiping_Click" >
                <img src="../../admin/Incentex_Used_Icons/shipping_information.png"  alt="World-Link System Control" />
                <span style="width:200px;">               
                   Save & Add New Shipping Address             
                </span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                     <div  class="collapsibleContainer" 
                    title="My Address Book" >
                        <asp:GridView ID="dtlShipping" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                            GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                            OnRowCommand="dtlShipping_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Id">
                                    <HeaderTemplate>
                                        <span>Company Contact ID</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblCompanyID" Text='<%# Eval("CompanyContactInfoID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Company">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkCompany" runat="server" CommandArgument="Company" CommandName="Sort"><span>Company</span></asp:LinkButton>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first">
                                            <asp:LinkButton ID="hypCompany" CommandName="EditRec" CommandArgument='<%# Eval("CompanyContactInfoID") %>'
                                                runat="server" ToolTip="Click here to Edit Shipping"><%# "&nbsp;" + Eval("CompanyName") %></asp:LinkButton>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Title">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkTitle" runat="server" CommandArgument="Title" CommandName="Sort"><span>Address Name</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("Title") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Airport">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkAirPort" runat="server" CommandArgument="Air Port" CommandName="Sort"><span>AirPort</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAirPort" Text='<%# Eval("AirPort") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn_space">
                                            <asp:ImageButton ID="lnkbtndelete" CommandName="DeleteRec" OnClientClick="return DeleteConfirmation();"
                                                CommandArgument='<%# Eval("CompanyContactInfoID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign" Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
