<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="Image.aspx.cs" Inherits="AssetManagement_Inventory_Image" Title="Image" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
<script type="text/javascript" language="javascript">
         $().ready(function() {
             $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                 objValMsg = $.xml2json(xml); $("#aspnetForm").validate(
                {
                    rules:
                    {
                        
                        ctl00$ContentPlaceHolder1$txtDescription: { required: true }
                    },
                    messages:
                    {

                        ctl00$ContentPlaceHolder1$txtDescription: { required: replaceMessageString(objValMsg, "Required", "Description") }
                    }
                });
             });
             $("#<%=lnkBtnSaveInfo.ClientID %>").click(function() {
                 return $('#aspnetForm').valid();
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
     <mb:MenuUserControl ID="menuControl" runat="server" />

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
                            <span class="input_label" style="width: 38%">Description</span>
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="20" CssClass="w_label"></asp:TextBox>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
                
                 <tr>
                    <td>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box">
                            <span class="input_label" style="width: 38%">Image</span>
                            &nbsp; <input type="file" id="DocFile" runat="server" />
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </td>
                </tr>
               
                <tr>
                    <td class="spacer10">
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSaveInfo" class="grey2_btn" runat="server" 
                            ToolTip="Save Basic Information" onclick="lnkBtnSaveInfo_Click" ><span>Add</span></asp:LinkButton>
                        &nbsp;&nbsp;
                         <asp:LinkButton ID="btnNext" class="grey2_btn" runat="server" 
                             onclick="btnNext_Click" ><span>Finish</span></asp:LinkButton>
                    </td>
                </tr>
                <tr><td>
                <div class="spacer20">
                    </div>
                    </td></tr>
                <tr><td>
                
                 </td></tr>
            </table>
        </div>
       <div class="form_pad">
                <div runat="server" id="dvTotalRecords">
                                <asp:GridView ID="gvEquipment" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnRowDataBound="gvEquipment_RowDataBound"
                                    OnRowCommand="gvEquipment_RowCommand" ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField Visible="False" >                                            
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("InventoryDocumentID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="DocumentDescription">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDocumentDescription" runat="server" CommandArgument="DocumentDescription" CommandName="Sort"><span >Image Description</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDocumentDescription" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDocumentDescription" runat="server" Value='<%# Eval("DocumentDescription")%>' />
                                                <asp:Label runat="server" ID="lblDocumentDescription" Text='<%# "&nbsp;" + Eval("DocumentDescription")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                         <asp:TemplateField SortExpression="DocumentName">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDocumentName" runat="server" CommandArgument="DocumentName" CommandName="Sort"><span >Image Name</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDocumentName" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDocumentName" runat="server" Value='<%# Eval("DocumentName")%>' />
                                                <asp:Label runat="server" ID="lblDocumentName" Text='<%# "&nbsp;" + Eval("DocumentName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Date">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkbtnDate" runat="server" CommandArgument="Date" CommandName="Sort"><span >Date</span></asp:LinkButton>
                                                <asp:PlaceHolder ID="placeholderDate" runat="server"></asp:PlaceHolder>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfDate" runat="server" Value='<%# Eval("UpdatedDate")%>' />
                                                <asp:Label runat="server" ID="lblDate" Text='<%# "&nbsp;" + Eval("UpdatedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="b_box" Width="15%" />
                                        </asp:TemplateField>
                                      <asp:TemplateField>
                                            <HeaderTemplate>
                                                <span class="centeralign">Delete</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn_space">
                                                    <asp:ImageButton ID="lnkbtndelete" runat="server" CommandName="del" OnClientClick="return DeleteConfirmation();"
                                                        CommandArgument='<%# Eval("InventoryDocumentID") %>' ImageUrl="~/Images/close.png" /></span>
                                           </ItemTemplate>
                                            <ItemStyle CssClass="b_box centeralign" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                 <div>
                   
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

