<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="MediaLocation.aspx.cs" Inherits="NewDesign_Admin_MediaLocation"  Title="Incentex | Media Location Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
      
    
        $().ready(function() {
         $(window).ValidationUI();
        
        $("#addNewModule").click(function() {
        
        $('#ctl00_ContentPlaceHolder1_ltPopupTitle').html("Add New Module");
        document.getElementById("ctl00_ContentPlaceHolder1_ddlPlacement").value="0";
        document.getElementById("ctl00_ContentPlaceHolder1_txtModuleName").value="";
                document.getElementById("ctl00_ContentPlaceHolder1_hfModuleId").value="0";
                AddNewModule();
             }); 
            });
            
       
       
       function AddNewModule() {

             ShowPopUp("add-module-Block","employess-content");
            
        }
        
         function ShowPopUp(MainDivTargetID) {
        
            $("#" + MainDivTargetID).css('top', '0').show();
            $(".fade-layer").show();
            SetPopUpAtTop();
             scrollTo(0,0);
        }
        
        
        
            
    </script>

  
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% }
       else
       { %>--%>
       <asp:ScriptManager ID="scpt1" runat="server"></asp:ScriptManager>
       <div id="div-mediasearch">
    <div id="container" class="cf filter-page">
        <%--<%} %>--%>
        <div class="narrowcolumn alignleft">
            <a id="addNewModule" href="javascript:;" class="blue-btn add-new">Add New Module</a>
            <div class="filter-block cf">
                <div class="title-txt">
                    <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help video" onclick="GetHelpVideo('Media Location Management','Media Location Module')">Help video</a></div>
                <div class="filter-form cf">
                    <ul class="cf">
                        <li>
                            <label class="select-txt filter-text">
                                Select one or more search criteria to open an existing record.
                            </label>
                        </li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchPlacement" runat="server" CssClass="default" TabIndex="1">
                                <asp:ListItem Selected="True">- Placement -</asp:ListItem>
                            </asp:DropDownList>
                        </span></li>
                        <li>
                            <asp:TextBox ID="txtSearchModuleName" runat="server" CssClass="input-field-all first-field default_title_text"
                                placeholder="Module Name" ToolTip="Module Name" TabIndex="2" MaxLength="200">
                            </asp:TextBox>
                        </li>
                        <li>
                            <asp:Button ID="btnSearchMedia" runat="server" CssClass="blue-btn add-new postback"
                                Text="Search for Module" TabIndex="4" OnClick="btnSearchMedia_Click" OnClientClick="ShowDefaultLoader();" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="filter-content">
                <div class="filter-header cf">
                    <span class="title-txt">Media Location Module</span> <em id="totalcount_em" runat="server"
                        visible="false"></em>
                    <%--      <div class="filter-search">
                        <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                            ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                        <asp:LinkButton ID="btnSearchGrid"  runat="server" CssClass="go-btn postback"
                            Enabled="true">
                            GO</asp:LinkButton>
                    </div>--%>
                </div>
                <div class="filter-tableblock cf">
                    <%-- OnRowCommand=""--%>
                    <asp:GridView ID="gvModuleName" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" CssClass="table-grid" OnRowCommand="gvModuleName_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="28%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblModuleName" runat="server" CommandArgument="Modulename" CommandName="Sort">Module Name</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lbModule" runat="server" Text='<%# Eval("ModuleName")%>' CommandName='EditRecord'
                                            CommandArgument='<%#Eval("Id").ToString() + "_" + Eval("Placement").ToString()  %>' OnClientClick="ShowDefaultLoader();"></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 postback" />
                                <ItemStyle CssClass="col1" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="28%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbPlacement" runat="server" CommandArgument="Placement" CommandName="Sort">Placement</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPlace" runat="server" Text='<%# Eval("Placement")%>' CommandName='EditRecord'
                                        CommandArgument='<%#Eval("Id").ToString() + "_" + Eval("Placement").ToString()  %>' OnClientClick="ShowDefaultLoader();"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2 postback" />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbPages" runat="server">Add Places</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPages" runat="server" Text='Add Places' CommandName='ShowPlace'
                                        CommandArgument='<%#Eval("Id").ToString() + "_" + Eval("Placement").ToString()  %>' OnClientClick="ShowDefaultLoader();"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2 postback" />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" CssClass="pagination alignright view-link postback cf"
                        OnClick="lnkViewAll_Click" >
                        VIEW ALL
                    </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft postback" runat="server"
                                ToolTip="Previous" OnClick="lnkPrevious_Click">
                            </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNext" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNext_Click" ToolTip="Next">
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <%--<% if (Request.IsLocal)
       { %>--%>
    </div>
    <%--<% }
       else
       { %>
    </section>
    <%} %>--%>
    <%-- Start Add Employee Popup --%>
    <div class="popup-outer" id="add-module-Block" style="display: none;">
        <div class="popupInner">
            <div class="employee-pop-block">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Media Location Management','Add New Module')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="aCloseAddModule">Close</a>
                <div class="employess-content emp-height">
                    <h2>
                        <%--<asp:Literal ID="ltPopupTitle" runat="server" Text="Add New Module">--%>
                        <asp:Label ID="ltPopupTitle" runat ="server" Text="Add New Module"></asp:Label> 
                       
                    </h2>
                    <div style="clear: both;"></div>
                    <div class="employee-form current-tab manualentry">
                        <div>
                            <div class="basic-form cf">
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Module Name</span>
                                            <asp:TextBox ID="txtModuleName" runat="server" TabIndex="7" CssClass="input-field-all first-field checkvalidation"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvModuleName" runat="server" ControlToValidate="txtModuleName"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddModule"
                                                ErrorMessage="">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Placement</span>
                                            
                                            <span class="select-drop medium-drop">
                                            <asp:DropDownList ID="ddlPlacement" runat="server" CssClass="default checkvalidation"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPlacement"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="AddModule"
                                                ErrorMessage="" InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                            </span>
                                            
                                            
                                        </label>
                                    </li>
                                </ul>
                            </div>
                            <div class="emp-btn-block">
                                <a id="lbAddPopupCancel" href="javascript:;" tabindex="20" class="cancel-popup gray-btn">Cancel</a>
                               <%-- <asp:LinkButton ID="lbAddPopupAddModule" runat="server" TabIndex="21" CssClass="blue-btn postback submit"
                                    ValidationGroup="AddModule" call="AddModule" OnClick="lbAddPopupAddModule_Click">
                                        Save
                                </asp:LinkButton>--%>
                                <asp:Button ID="lbAddPopupAddModule" runat="server" TabIndex="21" CssClass="blue-btn postback submit"
                                    ValidationGroup="AddModule" call="AddModule" OnClick="lbAddPopupAddModule_Click" Text="Save" />
                                <asp:HiddenField ID="hfModuleId" runat="server" />
                                <asp:HiddenField ID="hfModuleType" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- End Add Employee Popup --%>
    </div> 
</asp:Content>
