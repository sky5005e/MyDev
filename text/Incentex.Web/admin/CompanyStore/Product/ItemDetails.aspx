<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ItemDetails.aspx.cs" Inherits="admin_ProductStoreManagement_ItemDetails"
    Title="Product>> Item Details" %>

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
            padding-left: 55px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">
        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }

        function NameFormatChanged() {

            assigndesign();

        }

        function toPascalCase(str) {
            var arr = str.split(/\s|_/);
            for (var i = 0, l = arr.length; i < l; i++) {
                arr[i] = arr[i].substr(0, 1).toUpperCase() +
                         (arr[i].length > 1 ? arr[i].substr(1).toLowerCase() : "");
            }
            return arr.join(" ");
        }

        function FontFormatChanged() {
            assigndesign();

        }

    </script>

    <script runat="server">

        protected void ProcessClick_Handler(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
        }

    </script>

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        } 
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    onsubmit: false,
                    rules: {
                        ctl00$ContentPlaceHolder1$txtItemNumber: { required: true },
                        ctl00$ContentPlaceHolder1$txtItemPrdDescription: { required: true },
                        ctl00$ContentPlaceHolder1$ddlMasterNumber: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlItemNumberStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlColor: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlItemsize: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlStyle: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlAllowbackOrders: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtMinimumOrderAmt: { required: true, number: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlItemNumberStatus: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "Item Number Status")
                        },
                        ctl00$ContentPlaceHolder1$ddlColor: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "color")
                        },
                        ctl00$ContentPlaceHolder1$ddlItemsize: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "size")
                        },
                        ctl00$ContentPlaceHolder1$ddlStyle: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "style no")
                        },
                        ctl00$ContentPlaceHolder1$ddlMasterNumber: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "master item no")
                        },

                        ctl00$ContentPlaceHolder1$txtItemNumber:
                        {
                            required: replaceMessageString(objValMsg, "Required", "item number")

                        },
                        ctl00$ContentPlaceHolder1$txtItemPrdDescription:
                        {
                            required: replaceMessageString(objValMsg, "Required", "item Descriptions")

                        },
                        ctl00$ContentPlaceHolder1$ddlAllowbackOrders: {
                            NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "allow back orders")
                        },
                        ctl00$ContentPlaceHolder1$txtMinimumOrderAmt:
                        {
                            required: replaceMessageString(objValMsg, "Required", "minimum order amount "),
                            number: replaceMessageString(objValMsg, "Number", "")
                        }

                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$txtItemPrdDescription")
                            error.insertAfter("#divtxtItemPrdDescription");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlItemNumberStatus")
                            error.insertAfter("#dvItemNumberStatus");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlColor")
                            error.insertAfter("#dvColor");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlItemsize")
                            error.insertAfter("#dvItemsize");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStyle")
                            error.insertAfter("#dvStyle");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlMasterNumber")
                            error.insertAfter("#dvMasterNumber");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlAllowbackOrders")
                            error.insertAfter("#dvAllowbackOrders");
                        else
                            error.insertAfter(element);
                    }

                });

                /*End Change Event of all the dropdowns*/
                $("#<%=lnkAddItem.ClientID %>").click(function() {

                    return $('#aspnetForm').valid();
                });

            });
        });        
        
    </script>

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            assigndesign();

        }
    </script>

    <script type="text/javascript" language="javascript">

        function DeleteConfirmation() {
            if (confirm("Are you sure, you want to delete selected records ?") == true)
                return true;
            else
                return false;
        }
        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            var div = document.getElementById('dvSizePriority');
            if (!IsNumeric(txt.value)) {
                div.innerHTML = 'Please enter numeric value';
                txt.focus();
            }
            else {
                div.innerHTML = '';
            }

        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789";
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

    <script language="javascript" type="text/javascript">
        function ShowImagePopup(ImagePath) {
            var img = document.getElementById("imgPopImage");
            var path = '../../../UploadedImages/ProductImages/Thumbs/' + ImagePath;
            img.src = path;
            if (ImagePath != '') {
                $("#ctl00_ContentPlaceHolder1_pnlImage").show();
            }
        }
        function CloseImagePopup() {
            $("#ctl00_ContentPlaceHolder1_pnlImage").hide();
        }
    </script>

    <div class="form_pad">
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <div style="text-align: center; color: Red; font-size: larger;">
                            <asp:Label ID="lblmsg" runat="server"></asp:Label>
                        </div>
                    </td>
                    <td class="formtd">
                    </td>
                </tr>
                <tr>
                    <td class="formtd">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Style</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlStyle" runat="server" TabIndex="1" onchange="pageLoad(this,value);"
                                                    OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvStyle">
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Master Number</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlMasterNumber" runat="server" TabIndex="2" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvMasterNumber">
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Color</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlColor" runat="server" TabIndex="3" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvColor">
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Sold By</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlSoldBy" runat="server" TabIndex="4" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvSoldBy">
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
                                            <span class="input_label" style="padding-left: 4px;">Size Priority</span>
                                            <asp:TextBox ID="txtSizePriority" MaxLength="50" TabIndex="5" runat="server" CssClass="w_label"
                                                onchange="CheckNum(this.id)"></asp:TextBox>
                                            <div id="dvSizePriority" style="color: red; display: block; font-size: 11px; margin-bottom: -5px;
                                                padding-left: 55px; font-style: italic; margin-left: 31%;">
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
                                        <div class="form_box taxt_area clearfix">
                                            <span class="input_label alignleft" style="width: 39%!important;">Item Description</span>
                                            <div class="textarea_box alignright" style="width: 57%;">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                        class="scrollbottom"></a>
                                                </div>
                                                <asp:TextBox ID="txtItemPrdDescription" runat="server" TabIndex="6" TextMode="MultiLine"
                                                    CssClass="scrollme1" Height="70px">
                                                </asp:TextBox>
                                            </div>
                                            <div id="divtxtItemPrdDescription">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trallowforbackorder" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Allow for Back Orders</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlAllowbackOrders" runat="server" TabIndex="14" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvAllowbackOrders">
                                            </div>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                                <td></td>
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
                                            <span class="input_label" style="padding-left: 4px;">Item Number</span>
                                            <asp:TextBox ID="txtItemNumber" MaxLength="50" TabIndex="7" runat="server" CssClass="w_label"></asp:TextBox>
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Item Number Status</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlItemNumberStatus" runat="server" TabIndex="8" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvItemNumberStatus">
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
                                            <span class="input_label" style="padding-left: 4px;">Vender Item #</span>
                                            <asp:TextBox ID="txtVenderItemNumber" runat="server" TabIndex="9" CssClass="w_label"></asp:TextBox>
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Size</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlItemsize" runat="server" TabIndex="10" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvItemsize">
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
                                        <div class="form_box shipmax_in">
                                            <span class="input_label">Material / Style</span> <span class="custom-sel label-sel-small">
                                                <asp:DropDownList ID="ddlMaterialStyle" runat="server" TabIndex="11" onchange="pageLoad(this,value);">
                                                </asp:DropDownList>
                                            </span>
                                            <div id="dvMaterialStyle">
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
                                            <span class="input_label" style="padding-left: 4px;">Minimum Order Amount</span>
                                            <asp:TextBox ID="txtMinimumOrderAmt" MaxLength="10" TabIndex="8" runat="server" CssClass="w_label"></asp:TextBox>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="tdUpload" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label" style="padding-left: 4px;">Image</span>
                                            <asp:FileUpload ID="FileUploadItemimage" runat="server" />
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trNameFormat" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Name Format</span> <span class="custom-sel label-sel-small-Product">
                                                <asp:DropDownList ID="ddlNameFormat" runat="server" onchange="javascript:NameFormatChanged();">
                                                    <asp:ListItem Text="-select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="First Name" Value="FirstName"></asp:ListItem>
                                                    <asp:ListItem Text="Last Name" Value="LastName"></asp:ListItem>
                                                    <asp:ListItem Text="First Initial.LastName" Value="FirstInitial-LastName"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="form_bot_co">
                                            <span>&nbsp;</span></div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trFontFormat" runat="server">
                                <td>
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box">
                                            <span class="input_label">Font Format</span> <span class="custom-sel label-sel-small-Product">
                                                <asp:DropDownList ID="ddlFontFormat" runat="server" onchange="javascript:FontFormatChanged();">
                                                    <asp:ListItem Text="-select-" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="All Caps" Value="AllCaps"></asp:ListItem>
                                                    <asp:ListItem Text="Upper and Lower Case" Value="ULC"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
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
                    <td class="formtd" colspan="2">
                        <div id="divAddItem" runat="server" class="botbtn centeralign">
                            <asp:LinkButton ID="lnkAddItem" class="grey2_btn" runat="server" TabIndex="7" ToolTip="Add Item"
                                OnClick="lnkAddItem_Click">
                                <span runat="server" id="spanAddItem">Add Item</span><span runat="server" id="spanSaveitem"
                                    visible="false">Save Item</span></asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <h4>
                List of Item Details</h4>
            <div>
                <div>
                    <div style="text-align: center; color: Red; font-size: larger;">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                    <asp:GridView ID="gvItemDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" OnRowCommand="gvItemDetails_RowCommand"
                        OnRowDataBound="gvItemDetails_RowDataBound" RowStyle-CssClass="ord_content" OnRowEditing="gvItemDetails_RowEditing">
                        <Columns>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>ProductItemID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="2%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Id">
                                <HeaderTemplate>
                                    <span>Product ID</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProductId" Text='<%# Eval("ProductId") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check">
                                <HeaderTemplate>
                                    <span>
                                        <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;</span>
                                    <div class="corner">
                                        <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                    </div>
                                </HeaderTemplate>
                                <HeaderStyle CssClass="centeralign" />
                                <ItemTemplate>
                                    <span class="first">
                                        <asp:CheckBox ID="chkItem" runat="server" />&nbsp; </span>
                                    <div class="corner">
                                        <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" CssClass="b_box centeralign" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="MasterStyleName">
                                <HeaderTemplate>
                                    <span>
                                        <asp:LinkButton ID="lnkbtnMasterStyleName" runat="server" CommandArgument="MasterStyleName"
                                            CommandName="Sort">Master #</asp:LinkButton>
                                    </span>
                                    <asp:PlaceHolder ID="placeholderMasterStyleName" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyleName")%>'><%# (Eval("MasterStyleName").ToString().Length > 20) ? Eval("MasterStyleName").ToString().Substring(0, 20) + "..." : Eval("MasterStyleName") + "&nbsp;"%></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ProductStyle">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnStyle" runat="server" CommandArgument="ProductStyle" CommandName="Sort"><span>Style</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderStyle" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStyle" Text='<%# Eval("ProductStyle") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ItemNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                        CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblItemNumber" Text='<%# Eval("ItemNumber") %>' />
                                    <asp:HiddenField ID="hdnItemNumber" runat="server" Value='<%# Eval("ItemNumber") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="17%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ItemNoStatus">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemNoStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span>Status</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderItemNoStatus" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnLookupIcon" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IconPath")%>' />
                                    <asp:HiddenField ID="hdnStatusID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemNumberStatusID")%>' />
                                    <asp:LinkButton ID="lblStatus" runat="server" Text="" CommandName="StatusVhange"
                                        class="btn_space">
                                        <span class="btn_space">
                                            <img id="imgLookupIcon" style="height: 20px; width: 20px" runat="server" alt='Loading' /></span></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ItemColor">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblItemColor" ToolTip='<%# Eval("ItemColor")%>' Text='<%# (Eval("ItemColor").ToString().Length > 8) ? Eval("ItemColor").ToString().Substring(0, 8) + "..." : Eval("ItemColor") + "&nbsp;"%>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="9%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ItemSize">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="SizePriority">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSizePriority" runat="server" CommandArgument="SizePriority"
                                        CommandName="Sort"><span>Priority</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderSizePriority" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space">
                                        <asp:TextBox runat="server" ID="txtSizePriority" Text='<%# Eval("SizePriority") %>'
                                            Style="background-color: #303030; border: medium none; color: #FFFFFF; padding: 2px;
                                            width: 100%; text-align: center;" /></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="4%" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="ItemImage">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnItemImage" runat="server" CommandArgument="ItemImage" CommandName="Sort"><span>Image</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderItemImage" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnItemImage" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemImage")%>' />
                                    <span class="btn_space">
                                        <asp:Image ID="imgItemImage" Style="height: 20px; width: 20px" runat="server" alt='No Image' /></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box centeralign" Width="8%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Delete</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn_space">
                                        <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                            CommandArgument='<%# Eval("ProductItemID") %>' ImageUrl="~/Images/close.png" /></span>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box centeralign" Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="dvSave" runat="server" class="companylist_botbtn alignleft">
                    <asp:LinkButton ID="btnSavePriority" runat="server" TabIndex="0" CssClass="grey_btn"
                        OnClick="btnSavePriority_Click"><span>Save Priority</span>
                    </asp:LinkButton>
                </div>
                <div>
                    <div id="pagingtable" runat="server" class="alignright pagging">
                        <asp:LinkButton ID="lnkbtnPrevious" OnClick="lnkbtnPrevious_Click" class="prevb"
                            runat="server"> 
                        </asp:LinkButton>
                        <span>
                            <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                OnItemCommand="DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound"
                                RepeatLayout="Flow">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList></span>
                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Panel ID="pnlImage" runat="server" Style="display: none;">
            <div class="fancybox-overlay">
            </div>
            <div class="pp_pic_holder facebook" style="display: block; width: auto; height: auto;
                position: fixed; right: 20%; bottom: 20%;">
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
                            <div class="pp_content" style="height: auto; display: block;">
                                <div class="pp_loaderIcon" style="display: none;">
                                </div>
                                <div class="pp_fade" style="display: block;">
                                    <div id="pp_full_res">
                                        <div class="pp_inline clearfix">
                                            <div class="form_popup_box">
                                                <img id="imgPopImage" alt="popup image" src="../../../UploadedImages/ProductImages/1_Thumb.jpg"
                                                    height="120px" width="100px" />
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
    </div>
    <input id="hdnSubItemImage" type="hidden" value="0" runat="server" />
</asp:Content>
