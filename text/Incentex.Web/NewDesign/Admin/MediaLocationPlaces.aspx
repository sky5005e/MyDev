<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="MediaLocationPlaces.aspx.cs" Inherits="NewDesign_Admin_MediaLocationPlaces"
    Title="Incentex | Media Location Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
      
    
        $().ready(function() {
         $(window).ValidationUI();
        
        $("#addNewModule").click(function() {
          $('#ctl00_ContentPlaceHolder1_ltPopupTitle').html("Add New Place");
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

    <link href="../StaticContents/LightBox/colorbox.css" rel="stylesheet" type="text/css" />

    <script src="../StaticContents/LightBox/jquery.colorbox.js" type="text/javascript"></script>

    <script>
    
    
        
    
    $(document).ready(function () {
    
   
            
        //Examples of how to assign the Colorbox event to elements
        $(".group1").colorbox({ rel: 'group1' });
        $(".group2").colorbox({ rel: 'group2', transition: "fade" });
        $(".group3").colorbox({ rel: 'group3', transition: "none", width: "527", height: "670" });
        $(".group4").colorbox({ rel: 'group4', slideshow: true });
        $(".ajax").colorbox();
        $(".youtube").colorbox({ iframe: true, innerWidth: 800, innerHeight: 600 });
        $(".vimeo").colorbox({ iframe: true, innerWidth: 500, innerHeight: 409 });
        $(".iframe").colorbox({ iframe: true, width: "60%", height: "60%" });
        $(".inline").colorbox({ inline: true, width: "50%" });
        $(".callbacks").colorbox({
            onOpen: function () { alert('onOpen: colorbox is about to open'); },
            onLoad: function () { alert('onLoad: colorbox has started to load the targeted content'); },
            onComplete: function () { alert('onComplete: colorbox has displayed the loaded content'); },
            onCleanup: function () { alert('onCleanup: colorbox has begun the close process'); },
            onClosed: function () { alert('onClosed: colorbox has completely closed'); }
        });

        $('.non-retina').colorbox({ rel: 'group5', transition: 'none' })
        $('.retina').colorbox({ rel: 'group5', transition: 'none', retinaImage: true, retinaUrl: true });

        //Example of preserving a JavaScript event for inline calls.
        $("#click").click(function () {
            $('#click').css({ "background-color": "#f00", "color": "#fff", "cursor": "inherit" }).text("Open this window again and this message will still be here.");
            return false;
        });
    });
    </script>

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% }
       else
       { %>--%>
    <asp:ScriptManager ID="scpt1" runat="server">
    </asp:ScriptManager>
    <div id="div-mediasearch">
    <div id="container" class="cf filter-page">
        <%--<%} %>--%>
        <div class="narrowcolumn alignleft">
            <a id="addNewModule" href="javascript:;" class="blue-btn add-new">Add New Place</a>
            <div class="filter-block cf">
                <div class="title-txt">
                    <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help video" onclick="GetHelpVideo('Media Location Management','Places')">Help video</a></div>
                <div class="filter-form cf">
                    <ul class="cf">
                        <li>
                            <label class="select-txt filter-text">
                                Select one or more search criteria to open an existing record.
                            </label>
                        </li>
                        <li>
                            <asp:TextBox ID="txtSearchModuleName" runat="server" CssClass="input-field-all first-field default_title_text"
                                placeholder="Module Name" ToolTip="Module Name" TabIndex="2" MaxLength="200">
                            </asp:TextBox>
                        </li>
                        <li>
                            <asp:Button ID="btnSearchMedia" runat="server" CssClass="blue-btn add-new postback"
                                Text="Search For Place" TabIndex="4" OnClick="btnSearchMedia_Click" OnClientClick="ShowDefaultLoader();" />
                            <%----%>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="filter-content">
                <div class="filter-header cf">
                    <span class="title-txt">
                        <asp:Literal ID="ltPlaces" runat="server"></asp:Literal>
                    </span><em id="totalcount_em" runat="server" visible="false"></em>
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
                    <asp:GridView ID="gvPlaceName" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" CssClass="table-grid" OnRowCommand="gvPlaceName_RowCommand">
                        <%--OnRowCommand="gvPlaceName_RowCommand--%>
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="0%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblModulename" runat="server" CommandArgument="Modulename" CommandName="Sort">Place Name</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lbModule" runat="server" Text='<%# Eval("ModuleName")%>' CommandName='EditRecord'
                                            CommandArgument='<%#Eval("Id").ToString() %>' OnClientClick="ShowDefaultLoader();"></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 postback" />
                                <ItemStyle CssClass="col1" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbPageType" runat="server" CommandArgument="PageType" CommandName="Sort">Page Type</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lblPageType" runat="server" Text='<%# Eval("PageType")%>' CommandName='EditRecord'
                                            CommandArgument='<%#Eval("Id").ToString() %>' OnClientClick="ShowDefaultLoader();"></asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 postback" />
                                <ItemStyle CssClass="col1" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="20%">
                                <HeaderTemplate>
                                 <div style="width:100%;text-align:center;">
                                    <asp:LinkButton ID="lbScreeImage" runat="server" >Screen Image</asp:LinkButton>
                                    </div> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="width:100%;text-align:center;">
                                        <%--<img src="../StaticContents/img/search.png" width="30px" height="30px" />--%>
                                        <%#GetMediaImage(Eval("ThumbnailImage") == null ? "" : Eval("ThumbnailImage").ToString())%>
                                        
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 postback" HorizontalAlign="Center" />
                                <ItemStyle CssClass="col1" HorizontalAlign="Center"   />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" CssClass="pagination alignright view-link postback cf"
                        OnClick="lnkViewAll_Click">
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
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Media Location Management','Add New Place')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="aCloseAddModule">Close</a>
                <div class="employess-content emp-height">
                    <h2>
                        <%--<asp:Literal ID="ltPopupTitle" runat="server" Text="Add New Module">--%>
                        <asp:Label ID="ltPopupTitle" runat="server" Text="Add New Place"></asp:Label>
                    </h2>
                    <div style="clear: both;">
                    </div>
                    <div class="employee-form current-tab manualentry">
                        <div>
                            <div class="basic-form cf">
                                <ul class="left-form cf">
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Place Name</span>
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
                                            <span class="lbl-txt">URL</span>
                                            <asp:TextBox ID="txtUrl" runat="server" TabIndex="7" CssClass="input-field-all first-field"
                                                MaxLength="200"></asp:TextBox>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Screen Image</span>
                                            <asp:FileUpload ID="fuscreenimage" runat="server" />
                                            <br />
                                            <br />
                                            <a id="ShowImage" class='ajax' runat="server" style="padding-left: 129px;">Show Image</a>
                                        </label>
                                    </li>
                                    <li class="alignleft">
                                        <label>
                                            <span class="lbl-txt">Place Type</span>
                                            <asp:DropDownList ID="ddlPlaceType" runat="server" CssClass="default" TabIndex="1"
                                                Style="width: 249px;">
                                                <asp:ListItem Selected="True" Value="0">- Place Type -</asp:ListItem>
                                                <asp:ListItem>Page</asp:ListItem>
                                                <asp:ListItem>Pop-up</asp:ListItem>
                                            </asp:DropDownList>
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
                                    ValidationGroup="AddModule" call="AddModule" Text="Save" OnClick="lbAddPopupAddModule_Click" />
                                <%----%>
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
