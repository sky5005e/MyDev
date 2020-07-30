<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" 
    CodeFile="CheckOnSiteInventory.aspx.cs" Inherits="AssetManagement_RepairManagement_CheckOnSiteInventory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div style="text-align: center">
        <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
    </div>
    <div class="form_pad">
        <div class="form_table">        
                    <table class="dropdown_pad ">
                        <tr>
                            <td>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label" style="width: 38%">Company</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlCompany" runat="server" onchange="pageLoad(this,value);">
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
                                    <span class="input_label" style="width: 38%">Base Station</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlBaseStation" runat="server" onchange="pageLoad(this,value);">
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
                                    <span class="input_label" style="width: 38%">Product Category</span>
                                    <label class="dropimg_width">
                                        <span class="custom-sel label-sel-small">
                                            <asp:DropDownList ID="ddlProductCategory" runat="server" >
                                    </asp:DropDownList>
                                        </span>
                                    </label>
                                </div>
                                <div class="form_bot_co" onselectedindexchanged="ddlProductCategory_SelectedIndexChanged">
                                    <span>&nbsp;</span>
                                </div>
                            </td>
                        </tr>
                  
                        <tr>
                            <td class="spacer10">
                            </td>
                        </tr>
                       <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSearch" class="grey2_btn" runat="server" 
                            ToolTip="Search Basic Information" onclick="lnkBtnSearch_Click" ><span>Search</span></asp:LinkButton>
                        
                    </td>
                </tr>
                    </table>
        </div>
        
        <div class="form_pad">
                <div>  
                    <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                        CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowCommand="gvEquipment_RowCommand"
                        OnRowDataBound="gvEquipment_RowDataBound">
                        <Columns>
                            <asp:TemplateField Visible="False">                               
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEquipmentInventoryID" Text='<%# Eval("EquipmentInventoryID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField Visible="false">                                
                                <ItemTemplate>   
                                <asp:Label runat="server" ID="lblVendorID" Text='<%# Eval("VendorID") %>' />  
                                </ItemTemplate>
                               </asp:TemplateField>
                            
                            <asp:TemplateField SortExpression="PartNumber">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnPartNumber" runat="server" CommandArgument="PartNumber" CommandName="Sort"><span >Part #</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderPartNumber" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfPartNumber" runat="server" Value='<%# Eval("PartNumber")%>' />
                                    <asp:Label runat="server" ID="lblPartNumber" Text='<%# "&nbsp;" + Eval("PartNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="b_box" Width="25%" />
                            </asp:TemplateField>
                           
                            <asp:TemplateField SortExpression="ProductDescription">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnProductDescription" runat="server" CommandArgument="ProductDescription" CommandName="Sort"><span >Description</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderProductDescription" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfProductDescription" runat="server" Value='<%# Eval("ProductDescription")%>' />
                                    <asp:Label runat="server" ID="lblProductDescription" Text='<%# "&nbsp;" + Eval("ProductDescription")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="60%" />
                            </asp:TemplateField>
                             <asp:TemplateField SortExpression="CurrentInvenory">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnInStock" runat="server" CommandArgument="InStock" CommandName="Sort"><span >In Stock</span></asp:LinkButton>
                                    <asp:PlaceHolder ID="placeholderInStock" runat="server"></asp:PlaceHolder>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfInStock" runat="server" Value='<%# Eval("CurrentInvenory")%>' />
                                    <asp:Label runat="server" ID="lblInStock" Text='<%# "&nbsp;" + Eval("CurrentInvenory")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="g_box" Width="10%" />
                            </asp:TemplateField>
                             <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnInventory" runat="server" ><span >Pick Qty</span></asp:LinkButton>
                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <span style="height:26px; text-align: center;">
                                                <asp:TextBox ID="txtPickQty" runat="server" Style="background-color: #303030; border: medium none;
                                                    vertical-align: middle; text-align:center; color: #ffffff; width: 65px; padding: 2px" onchange="CheckNum(this.id)"
                                                    MaxLength="2" BackColor="#303030" Text='0'></asp:TextBox>
                                            </span>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="b_box" Width="5%" />
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>   
                  <div runat="server" class="alignleft ">
                        <asp:LinkButton ID="lnkBtnPickInventory" class="grey2_btn" runat="server" 
                            ToolTip="Pick Inventory" onclick="lnkBtnPickInventory_Click" ><span>Pick Inventory</span></asp:LinkButton>
                        
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
    </div>
    
    
</asp:Content>
