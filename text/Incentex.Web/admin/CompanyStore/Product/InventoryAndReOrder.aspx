<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="InventoryAndReOrder.aspx.cs" Inherits="admin_ProductStoreManagement_InventoryAndReOrder"
    Title="Product >> Inventory & Re-Order" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="dvLoader" style="display: none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../../../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <mb:MenuUserControl ID="menuControl" runat="server" />

    <script type="text/javascript" language="javascript">
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

                $(function() {
                    $(".datepicker1").datepicker({
                        buttonText: 'DatePicker',
                        showOn: 'button',
                        buttonImage: '../../../images/calender-icon.jpg',
                        buttonImageOnly: true
                    });
                });
            }
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
                    rules: {
                        ctl00$ContentPlaceHolder1$lblStyle: { required: true },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNo: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlItemNumber: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$lblStyle: {
                            required: replaceMessageString(objValMsg, "Required", "style")
                        },
                        ctl00$ContentPlaceHolder1$ddlMasterItemNo: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "master item number")
                        },
                        ctl00$ContentPlaceHolder1$ddlItemNumber: {
                            NotequalTo: "<br/>" + replaceMessageString(objValMsg, "NotEqualTo", "item number")
                        }
                    }

                });

                $("#<%=lnkAddItem.ClientID %>").click(function() {
                    return $('#aspnetForm').valid();
                });
                
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

    <div class="form_pad">
        <div>
            <asp:UpdatePanel runat="server" ID="upAdd">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlAddItem">
                        <table class="dropdown_pad form_table">
                            <tr>
                                <td colspan="2">
                                    <div style="text-align: center; color: Red; font-size: larger;">
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlMasteritemNo" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box" style="width: 400px">
                                                    <span class="input_label">Master Item Number</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlMasterItemNo" TabIndex="1" runat="server" AutoPostBack="true"
                                                            onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlMasterItemNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="upnlItemNumber" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box" style="width: 400px">
                                                    <span class="input_label">Item Number</span> <span class="custom-sel label-sel-small">
                                                        <asp:DropDownList ID="ddlItemNumber" TabIndex="2" AutoPostBack="true" runat="server"
                                                            onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlItemNumber_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Item Color</span>
                                                    <asp:Label ID="lblItemColor" runat="server" TabIndex="3" CssClass="w_label"></asp:Label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Size</span>
                                                    <asp:Label ID="lblSize" runat="server" TabIndex="4" CssClass="w_label"></asp:Label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Available</span>
                                                    <asp:TextBox ID="txtInventory" MaxLength="100" TabIndex="5" runat="server" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">Re-Order Point</span>
                                                    <asp:TextBox ID="txtReOrderpoint" MaxLength="100" TabIndex="6" runat="server" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="formtd">
                                    <div>
                                        <div class="form_top_co">
                                            <span>&nbsp;</span></div>
                                        <div class="form_box taxt_area clearfix">
                                            <span class="input_label alignleft" style="width: 39%!important;">Product Description</span>
                                            <div class="textarea_box alignright" style="width: 52%;">
                                                <div class="scrollbar">
                                                    <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                        class="scrollbottom"></a></label>
                                                </div>
                                                <asp:TextBox ID="txtPrdDescription" runat="server" TextMode="MultiLine" CssClass="scrollme1"
                                                    Height="70px"></asp:TextBox>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </div>
                                </td>
                                <td class="formtd">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box">
                                                    <span class="input_label">On-Order</span>
                                                    <asp:TextBox ID="txtOnOrder" MaxLength="15" TabIndex="7" runat="server" CssClass="w_label"></asp:TextBox>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td class="formtd">
                                    <div id="Div1" visible="false" runat="server">
                                        <asp:UpdatePanel ID="upnldtyle" runat="server">
                                            <ContentTemplate>
                                                <div class="form_top_co">
                                                    <span>&nbsp;</span></div>
                                                <div class="form_box" style="width: 400px">
                                                    <span class="input_label">Style</span>
                                                    <asp:Label ID="lblStyle" runat="server" TabIndex="0" CssClass="w_label"></asp:Label>
                                                </div>
                                                <div class="form_bot_co">
                                                    <span>&nbsp;</span></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="botbtn centeralign">
                                        <asp:LinkButton ID="lnkAddItem" class="grey2_btn" runat="server" ToolTip="Save Item"
                                            TabIndex="11" OnClick="lnkAddItem_Click"><span>Save Item</span></asp:LinkButton>
                                        <asp:LinkButton ID="lnkEmailNotification" class="grey2_btn" runat="server" ToolTip="Add Item"
                                            OnClick="lnkEmailNotification_Click"><span>Email Notification</span></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <h4>
                List of Item Details</h4>
            <asp:UpdatePanel runat="server" ID="upnlgvInventroyOrder">                
                <ContentTemplate>
                    <div>
                        <div>
                            <div style="text-align: center; color: Red; font-size: larger;">
                                <asp:Label ID="lblmsg" runat="server">
                                </asp:Label>
                            </div>
                            <asp:GridView ID="gvInventroyOrder" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvInventroyOrder_RowCommand"
                                OnRowDataBound="gvInventroyOrder_RowDataBound" OnRowEditing="gvInventroyOrder_RowEditing">
                                <RowStyle CssClass="ord_content" />
                                <Columns>
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
                                                <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp; </span>
                                            <div class="corner">
                                                <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" CssClass="b_box centeralign" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>ProductItemInventoryID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProductItemInventoryID" Text='<%# Eval("ProductItemInventoryID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False" HeaderText="Id">
                                        <HeaderTemplate>
                                            <span>ProductItemID</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblProductItemID" Text='<%# Eval("ProductItemID") %>' />
                                        </ItemTemplate>
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
                                            <asp:HiddenField runat="server" ID="hdnMasterStyleID" Value='<%# Eval("MasterStyleID") %>' />
                                            <span>
                                                <asp:LinkButton ID="hypStyle" CommandName="Edit" runat="server" ToolTip='<%# Eval("MasterStyleName")%>'><%# (Eval("MasterStyleName").ToString().Length > 18) ? Eval("MasterStyleName").ToString().Substring(0, 18) + "..." : Eval("MasterStyleName")%></asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ProductstyleName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnStyle" runat="server" CommandArgument="ProductstyleName"
                                                CommandName="Sort"><span>Style</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderStyle" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStyle" ToolTip='<%# Eval("ProductstyleName") %>'
                                                Text='<%# (Eval("ProductstyleName").ToString().Length > 15) ? Eval("ProductstyleName").ToString().Substring(0, 15) + "..." : Eval("ProductstyleName") + "&nbsp;"%>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemNumber">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemNumber" runat="server" CommandArgument="ItemNumber"
                                                CommandName="Sort"> <span >Item #</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemNumber" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnItemNumber" Value='<%# Eval("ItemNumber") %>' />
                                            <span style="height: 26px; text-align: center;">
                                                <asp:DropDownList runat="server" onchange="pageLoad(this,value);" OnSelectedIndexChanged="ddlItemNumber_SelectedIndexChanged1"
                                                    AutoPostBack="true" ID="ddlItemNumber" Style="border: medium none; background-color: #303030;
                                                    color: #ffffff; width: 100px; padding: 2px;">
                                                </asp:DropDownList>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemColor">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemColor" runat="server" CommandArgument="ItemColor" CommandName="Sort"><span>Color</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemColor" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:Image ID="imgColor" runat="server" Height="20" Width="20" />
                                            </span>
                                            <asp:Label runat="server" ID="lblItemColor" Visible="false" Text='<%# Eval("ItemColor") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ItemSize">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnItemSize" runat="server" CommandArgument="ItemSize" CommandName="Sort"><span>Size</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderItemSize" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="centeralign" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemSize" Text='<%# Eval("ItemSize") %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box centeralign" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Inventory">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnInventory" runat="server" CommandArgument="Inventory" CommandName="Sort"><span >Available</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderInventory" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span style="height: 26px; text-align: center;">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    vertical-align: middle; text-align: center; color: #ffffff; width: 50px; padding: 2px"
                                                    onchange="CheckNum(this.id)" MaxLength="10" BackColor="#303030" ID="txtInventorygrid"
                                                    Text='<%# Eval("Inventory") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ReOrderPoint">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnIReOrderPoint" runat="server" CommandArgument="ReOrderPoint"
                                                CommandName="Sort"><span >Re-Order</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderReorderPoint" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span style="height: 26px; text-align: center;">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    text-align: center; vertical-align: middle; color: #ffffff; width: 50px; padding: 2px"
                                                    onchange="CheckNum(this.id)" MaxLength="10" BackColor="#303030" ID="txtReOrderPointgrid"
                                                    Text='<%# Eval("ReOrderPoint") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="OnOrder">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnIOnOrder" runat="server" CommandArgument="OnOrder" CommandName="Sort"><span >On-Order</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderOnOrder" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span style="height: 26px; text-align: center;">
                                                <asp:TextBox runat="server" Style="background-color: #303030; border: medium none;
                                                    text-align: center; vertical-align: middle; color: #ffffff; width: 50px; padding: 2px"
                                                    onchange="CheckNum(this.id)" MaxLength="10" BackColor="#303030" ID="txtOnOrderGrid"
                                                    Text='<%# Eval("OnOrder") %>'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="InventoryArriveOn">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnInventoryArriveOn" runat="server" CommandArgument="InventoryArriveOn"
                                                CommandName="Sort"><span >To Arrive On</span></asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderInventoryArriveOn" runat="server"></asp:PlaceHolder>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblInventoryArriveOn" Text='<%# "&nbsp;" + Convert.ToString(Convert.ToString(Eval("InventoryArriveOn")) != "" && Eval("InventoryArriveOn") != "1/1/1900 12:00:00 AM" ? Convert.ToDateTime(Eval("InventoryArriveOn")).ToString("MM/dd/yyyy") : "") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="g_box" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <span>Delete</span>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="btn_space">
                                                <asp:ImageButton ID="lnkbtndelete" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                    CommandArgument='<%# Eval("ProductItemInventoryID") %>' runat="server" ImageUrl="~/Images/close.png" /></span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="ord_header" />
                            </asp:GridView>
                        </div>
                        <div>
                            <div class="companylist_botbtn alignleft">
                                <asp:LinkButton ID="btnSaveStatus" runat="server" TabIndex="0" CssClass="grey_btn"
                                    OnClick="btnSaveStatus_Click"><span>Save</span>
                                </asp:LinkButton>
                            </div>
                            <div id="pagingtable" runat="server" class="alignright pagging">
                                <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <span>
                                    <asp:DataList ID="DataList2" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList></span>
                                <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
