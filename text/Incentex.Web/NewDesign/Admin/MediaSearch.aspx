<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MediaSearch.aspx.cs" Inherits="NewDesign_Admin_MediaSearch"
    MasterPageFile="~/NewDesign/FrontMasterPage.master" Title="incentex | Document & Media Storage Management" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/NewDesign/UserControl/CustomDropDown.ascx" TagName="CustomDropDown"
    TagPrefix="cdd" %>
<%@ Register Src="~/NewDesign/UserControl/TextBoxControl.ascx" TagName="TextBoxControl"
    TagPrefix="cdd" %>
<%@ Reference Control="~/NewDesign/UserControl/CustomDropDown.ascx" %>
<%@ Reference Control="~/NewDesign/UserControl/TextBoxControl.ascx" %>
<%@ Reference Control="~/NewDesign/UserControl/DropDownControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
    
          function ShowVideo(VidUrl) {
            $("#video-block").css('top', '0');
            $(".fade-layer").show();
            $("#video-block").show();
            var videoURL =VidUrl;
            $("#ctl00_ContentPlaceHolder1_iframeVideo").attr("src", videoURL);
        }
        function CloseVideo() {
            $(".fade-layer").hide();
            $("#ctl00_ContentPlaceHolder1_iframeVideo").attr("src", '');
        }
        
        

        function SetPopUpAtTop() {
            $('.popupInner').each(function () {
                //$(this).css('margin-top', ($(this).parent('.popup-outer').height() - $(this).height()) / 2)
                $(this).css('margin-top', '78px');
                
            });
          }
          
          
          
         
     function ShowPopUp(MainDivTargetID, PopUpDivTargetID) {
            $("#" + MainDivTargetID).css('top', '0');
            $("#" + PopUpDivTargetID).show();
            $(".fade-layer").show();
            SetPopUpAtTop();
             scrollTo(0,0);
        }
        
          function ValidationForComp(chkbox)
        {
        var cnt = $("#ctl00_ContentPlaceHolder1_cbCompany input:checked").length;
    
            if(cnt>0 ) // false me it will be converted to true
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtCompany").value="- Selected -";
            }
            else
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtCompany").value="- Company -";
            }
            //
            if(cnt==1 && chkbox.checked==true)
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtCompany").value="- Company -";
            }
            
            if(cnt==0 && chkbox.checked==false)
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtCompany").value="- Selected -";
            }
        }
        
        function ValidationForWork(chkbox)
        {
        var cnt = $("#ctl00_ContentPlaceHolder1_cbWorkGroup input:checked").length;
       
            if(cnt>0)
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtWorkgroups").value="- Selected -";
            }
            else
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtWorkgroups").value="- Workgroup -";
            }
            
             if(cnt==1 && chkbox.checked==true)
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtWorkgroups").value="- Workgroup -";
            }
            
            if(cnt==0 && chkbox.checked==false)
            {
            document.getElementById("ctl00_ContentPlaceHolder1_txtWorkgroups").value="- Selected -";
            }
        }
        
        function ValidationForMultiSelection(chkbox,textboxid,chkboxid,defaultvalue)
        {
        
        var cnt = $("#"+ chkboxid +" input:checked").length;
     
            if(cnt>0)
            {
            document.getElementById(textboxid).value="- Selected -";
            }
            else
            {
            document.getElementById(textboxid).value=defaultvalue;
            }
            
             if(cnt==1 && chkbox.checked==true)
            {
            document.getElementById(textboxid).value=defaultvalue;
            }
            
            if(cnt==0 && chkbox.checked==false)
            {
            document.getElementById(textboxid).value="- Selected -";
            }
        }
        
        
        
        
      $().ready(function() {
            $(window).ValidationUI();
            
             $("#ctl00_ContentPlaceHolder1_txtWorkgroups").attr("autocomplete", "off");
             $("#ctl00_ContentPlaceHolder1_txtCompany").attr("autocomplete", "off");
                 

//          $(".postback").click(function() {
//              $(".progress-layer").show();
//          });


          
          
          
         $(".autotext").change(function () {
                ShowDefaultLoader();
        });
          $("#ctl00_ContentPlaceHolder1_PanalForModuleDoc input").click(function () {
        ShowDefaultLoader();
        });
        
        $("#ctl00_ContentPlaceHolder1_PanelForHelpModule input").click(function () {
        ShowDefaultLoader();
        });
        
      });
    </script>

    <link href="../StaticContents/LightBox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="../StaticContents/LightBox/jquery.colorbox.js" type="text/javascript"></script>
    <script type="text/javascript">
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="media-div-search">
        <%--<% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% }
       else
       { %>--%>
        <div id="container" class="cf filter-page">
            <%--<%} %>--%>
            <div class="narrowcolumn alignleft">
                <div class="filter-block cf">
                    <div class="title-txt">
                        <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help video" onclick="GetHelpVideo('Document and Media Storage','Document and Media Search');">Help
                            video</a></div>
                    <div class="filter-form cf">
                        <ul class="cf">
                            <li>
                                <label class="select-txt filter-text">
                                    Select one or more search criteria to open an existing Media.
                                </label>
                            </li>
                            <li><span class="select-drop filter-drop">
                                <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="default" TabIndex="1">
                                    <asp:ListItem Selected="True">- Company -</asp:ListItem>
                                </asp:DropDownList>
                            </span></li>
                            <li><span class="select-drop filter-drop">
                                <asp:DropDownList ID="ddlSearchWorkGroup" runat="server" CssClass="default" TabIndex="2">
                                    <asp:ListItem Selected="True">- Workgroup -</asp:ListItem>
                                </asp:DropDownList>
                            </span></li>
                            <li><span class="select-drop filter-drop">
                                <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="default" TabIndex="3">
                                </asp:DropDownList>
                            </span></li>
                            <li>
                                <asp:Button ID="btnSearchMedia" runat="server" CssClass="blue-btn add-new postback"
                                    Text="Search for Media" OnClick="btnSearchMedia_Click" TabIndex="4" />
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="widecolumn alignright">
                <div class="filter-content">
                    <div class="filter-header cf">
                        <span class="title-txt">Document and Media Search</span> <em id="totalcount_em" runat="server"
                            visible="false"></em>
                        <div class="filter-search">
                            <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                                ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" TargetControlID="txtSearchGrid"
                                runat="server" ServiceMethod="GetMediaSearchKeywords" ServicePath="~/NewDesign/UserPages/WSUser.asmx"
                                MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="true" CompletionListCssClass="CompletionListCssClassSearchTags"
                                CompletionListItemCssClass="CompletionListItemCssClassSearchTags" CompletionListHighlightedItemCssClass="CompletionListCssClassSelectedSearchTags" />
                            <asp:LinkButton ID="btnSearchGrid" OnClick="btnSearchGrid_Click" runat="server" CssClass="go-btn postback"
                                Enabled="true">
                            GO</asp:LinkButton>
                        </div>
                    </div>
                    <div class="filter-tableblock cf">
                        <%-- OnRowCommand=""--%>
                        <asp:GridView ID="gvMediaSearch" runat="server" AutoGenerateColumns="false" GridLines="None"
                            Visible="true" CssClass="table-grid" OnRowCommand="gvMediaSearch_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="24%">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbCompany" runat="server" CommandArgument="Company" CommandName="Sort">Company</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <asp:LinkButton ID="lbCompany" runat="server" Text='<%# Eval("Company")==null?"":Eval("Company")%>'
                                                CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("mediafileid").ToString() %>'></asp:LinkButton>
                                        </span>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col1 postback" />
                                    <ItemStyle CssClass="col1 GridRowHeight" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="24%">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbWorkgroupHeader" runat="server" CommandArgument="Workgroup"
                                            CommandName="Sort">Workgroup</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbWorkgroup" runat="server" Text='<%# Eval("Workgroup")==null?"":Eval("Workgroup")%>'
                                            CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("mediafileid").ToString() %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col2 postback" />
                                    <ItemStyle CssClass="col2 GridRowHeight" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="22%">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbLocation1" runat="server" CommandArgument="Location1" CommandName="Sort">Module</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                          <asp:LinkButton ID="lblLocation1" runat="server" Text='<%# Eval("Location1")==null?"":Eval("Location1")%>'
                                            CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("mediafileid").ToString() %>'></asp:LinkButton>
                                       
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col2 postback" />
                                    <ItemStyle CssClass="col2 GridRowHeight" />
                                </asp:TemplateField>
                                 <asp:TemplateField ItemStyle-Width="22%">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbLocation2" runat="server" CommandArgument="Location2" CommandName="Sort">Place</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lblLocation2" runat="server" Text='<%# Eval("Location2")==null?"":Eval("Location2")%>'
                                            CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("mediafileid").ToString() %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col2 postback" />
                                    <ItemStyle CssClass="col2 GridRowHeight" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="8%">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblView" runat="server" CommandArgument="View" CommandName="Sort">View</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblView" runat="server" Text='<%#(Eval("View"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col3 postback" />
                                    <ItemStyle CssClass="col3 " />
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblIssuanceStatusHeader" runat="server" CommandArgument="Status"
                                            CommandName="Sort">Status</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    
                                        <asp:Panel ID="pnlStatus" runat="server" >
                                            <div class="apple_check">
                                                <label class="apple_checkbox">
                                                    <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# GetStatus(Convert.ToString(Eval("Status")))  %>'
                                                        AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged"></asp:CheckBox>&nbsp;</label></div>
                                        </asp:Panel>
                                        <asp:HiddenField ID="hfMediaAssigned" runat="server" Value='<%# (Eval("MediaAssignedId")) %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="col4" />
                                    <ItemStyle CssClass="col4 " />
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
        
        <%-- Start Add Media Popup --%>
        <div id="add-media-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Assign Media')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="aCloseAddEmployee">Close</a>
                <div class="employee-pop-block">
                    <div class="employess-content" style="border:0px;">
                        <h2>
                            Assign Media</h2>
                        <div style="clear: both;">
                        </div>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <div class="mediatopcontent">
                                        <table cellpadding="0" cellspacing="0" width="90%" border="1">
                                            <tr>
                                                <td width="43%" valign="top">
                                                    <asp:Literal ID="ltmediaimage" runat="server"></asp:Literal>
                                                    <asp:HiddenField ID="hfMediaType" runat="server" />
                                                </td>
                                                <td valign="top" id="tdfiledetail">
                                                    <span style="font-weight: bold">
                                                        <asp:Literal ID="ltfilename" runat="server"></asp:Literal></span>
                                                    <br />
                                                    <br />
                                                    <asp:Literal ID="ltmediapopupdate" runat="server"></asp:Literal>
                                                    <br />
                                                    <asp:Literal ID="ltMediaPopupView" runat="server"></asp:Literal>
                                                    Views
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="mediathumbimage">
                                        <table cellpadding="0" cellspacing="0" border="1">
                                            <tr>
                                                <td width="50%" valign="top" align="right">
                                                    <asp:Literal ID="ltThumbImage" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Company</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtCompany" ReadOnly="true" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        Text="- Company -" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"></asp:TextBox>
                                                   <%-- <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Select Document"
                                                        Display="None" CssClass="error" ControlToValidate="txtCompany" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Company -"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left">
                                                <asp:PopupControlExtender ID="TextBox1_PopupControlExtender" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtCompany" PopupControlID="Panel1"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="Panel1" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbCompany" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                    <asp:HiddenField ID="hfcomp" runat="server" />
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Workgroup</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtWorkgroups" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Workgroup -"></asp:TextBox>
                                                   <%-- <asp:RequiredFieldValidator ID="rfvWorkGroup" runat="server" ErrorMessage="Select Workgroup"
                                                        Display="None" CssClass="error" ControlToValidate="txtWorkgroups" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Workgroup -"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left">
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtWorkgroups" PopupControlID="Panel2"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="Panel2" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbWorkGroup" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Placement</span> <span class="select-d"></span><span class="select-drop medium-drop"
                                                    style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlPlacement" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;" OnSelectedIndexChanged="ddlPlacement_SelectedIndexChanged"
                                                        AutoPostBack="true" onchange="ShowDefaultLoader();">
                                                        <asp:ListItem Selected="True">- Placement -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvPlacement" runat="server" ErrorMessage="Select Placement"
                                                        Display="None" CssClass="error" ControlToValidate="ddlPlacement" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft" id="lidocmain" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Module Name</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:TextBox ID="txtModuleForDoc" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Module -"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvdocmain" runat="server" ErrorMessage="Select Document"
                                                        Display="None" CssClass="error" ControlToValidate="txtModuleForDoc" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Module -"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left">
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtModuleForDoc" PopupControlID="PanalForModuleDoc"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanalForModuleDoc" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="CbModuleForDoc" runat="server" Style="font-size: 100%; font-family: Sans-Serif;"
                                                        AutoPostBack="true" OnSelectedIndexChanged="CbModuleForDoc_SelectedIndexChanged"
                                                        OnClientClick="ShowDefaultLoader()">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft" id="lidocsub" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Place</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:TextBox ID="txtPlaceForDoc" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Place -"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Document Section"
                                                        Display="None" CssClass="error" ControlToValidate="txtPlaceForDoc" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Place -"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left">
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtPlaceForDoc" PopupControlID="PanelPlaceForDoc"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanelPlaceForDoc" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbPlacefordoc" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft" id="liassetmain" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Module Name</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:TextBox ID="txtModuleForHelp" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Module -"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select Help Video"
                                                        Display="Dynamic" CssClass="error" ControlToValidate="txtModuleForHelp" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Module -"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left;">
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtModuleForHelp" PopupControlID="PanelForHelpModule"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanelForHelpModule" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="CbModuleForHelp" runat="server" Style="font-size: 100%; font-family: Sans-Serif;"
                                                        AutoPostBack="true" OnSelectedIndexChanged="CbModuleForHelp_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft" id="liassetsub" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Place</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:TextBox ID="txtPlaceForHelp" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Place -"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Select Help Video"
                                                        Display="None" CssClass="error" ControlToValidate="txtPlaceForHelp" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Place -"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left;">
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtPlaceForHelp" PopupControlID="PanelPlaceForHelpVideo"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanelPlaceForHelpVideo" runat="server" Height="116px" Width="275px"
                                                    BorderStyle="Solid" BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto"
                                                    BackColor="#F5F5F5" Style="display: none; margin-top: 27px; border-top: none;"
                                                    BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbPlaceForHelp" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft" id="liloginpopup" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Place</span> <span class="select-drop warranty-mid-drop popup"
                                                    id="Span1" runat="server" data-content="add-media-block;add-media-block .emp-content">
                                                    <asp:TextBox ID="txtPlaceforLogin" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Place -"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Select Login Popup Page"
                                                        Display="None" CssClass="error" ControlToValidate="txtPlaceforLogin" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Place -"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left;">
                                                <asp:PopupControlExtender ID="PanelForLogin" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtPlaceforLogin" PopupControlID="PanelPlaceforLogin"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanelPlaceforLogin" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbPlaceForLogin" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Search Tags
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtMediaSearchTag" runat="server" class="input-field-all checkvalidation"
                                                    TabIndex="15" MaxLength="500" ToolTip="Separate each with space"></asp:TextBox>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Publish Start<br />
                                                    Date</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtPubStartDate" runat="server" class="input-field-all setDatePicker"
                                                    TabIndex="15" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Publish End<br />
                                                    Date</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtPubEndDate" runat="server" class="input-field-all setDatePicker"
                                                    TabIndex="15" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Folder</span> <span class="select-d"></span><span class="select-drop medium-drop">
                                                    <asp:DropDownList ID="ddlMediaFolder" runat="server" CssClass="default checkvalidation">
                                                        <asp:ListItem Selected="True">- Placement -</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft" id="li1" runat="server">
                                            <label>
                                                <span class="lbl-txt">Password</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtMedoaPassword" TextMode="Password" runat="server" class="input-field-all checkvalidation"
                                                        TabIndex="15" MaxLength="500"></asp:TextBox>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <div class="emp-btn-block">
                                        <a id="lbAddPopupCancel" href="javascript:;" tabindex="20" class="cancel-popup gray-btn">
                                            Cancel</a>
                                        <asp:Button ID="btnAssignedMedia" runat="server" CssClass="blue-btn submit" ValidationGroup="MediaAssigned"
                                            call="MediaAssigned" OnClick="btnAssignedMedia_Click" Visible="true" />
                                        <asp:HiddenField ID="hfmediaid" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfMediaAssignedid" runat="server" Value="0" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Add Media Popup --%>
        <%-- Start assign to store product --%>
        <div id="add-assigntostoreproduct" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Assign image to store')">
                    Help Video</a>
                <%--<a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetDocumentLink('Employee Uniform','Ramp Agent Uniforms')">Help Video</a>--%>
                <a href="javascript:;" class="close-btn" id="a4">Close</a>
                <div class="employee-pop-block">
                    <div class="employess-content" style="border:0px;">
                        <h2>
                            Assign Image To Store Product</h2>
                        <div style="clear: both;">
                        </div>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;
                            top: 0px; position: relative;">
                            <div>
                                <div class="basic-form cf">
                                    <div class="mediatopcontent">
                                        <table cellpadding="0" cellspacing="0" width="90%" border="1">
                                            <tr>
                                                <td width="50%" valign="top">
                                                    <img id="imgProductImage" height="144" width="144" runat="server" />
                                                </td>
                                                <td valign="top" id="ProductDetail">
                                                    <span style="font-weight: bold">
                                                        <asp:Literal ID="ltfilenameforstore" runat="server"></asp:Literal></span>
                                                    <br />
                                                    <br />
                                                    <asp:Literal ID="ltmediapopupdateforstore" runat="server"></asp:Literal>
                                                    <br />
                                                    <asp:Literal ID="ltMediaPopupViewforstore" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Master Item #</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtMasterStyleName" runat="server" class="input-field-all checkvalidation autotext"
                                                        TabIndex="15" MaxLength="200" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please enter file name"
                                                        Display="None" CssClass="error" ControlToValidate="txtMasterStyleName" ValidationGroup="MediaAssignedToStore"></asp:RequiredFieldValidator>
                                                    <asp:AutoCompleteExtender ID="AutoCompExtForMasterStyle" TargetControlID="txtMasterStyleName"
                                                        runat="server" ServiceMethod="GetMasterItemDetail" ServicePath="~/NewDesign/UserPages/WSUser.asmx"
                                                        MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="true" CompletionListCssClass="CompletionListCssClass"
                                                        CompletionListItemCssClass="CompletionListItemCssClass" CompletionListHighlightedItemCssClass="CompletionListCssClassSelected" />
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Company</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtCompanyforassignimage" ReadOnly="true" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        Text="- Company -" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"></asp:TextBox>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Select Document"
                                                        Display="None" CssClass="error" ControlToValidate="txtCompany" ValidationGroup="MediaAssignedToStore"
                                                        SetFocusOnError="True" InitialValue="- Company -"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left;">
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtCompanyforassignimage"
                                                    PopupControlID="Panel3" OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="Panel3" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbCompanyforassignimage" runat="server" CssClass="masoom" Style="font-size: 100%;
                                                        font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Workgroup</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtWorkgroupForAssignImage" runat="server" class="input-field-all checkvalidation multiselectdropdown"
                                                        ReadOnly="true" Style="background: url('../StaticContents/img/custorm-drop-bg.png') no-repeat scroll right top rgba(0, 0, 0, 0)"
                                                        Text="- Workgroup -"></asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvWorkGroup" runat="server" ErrorMessage="Select Workgroup"
                                                        Display="None" CssClass="error" ControlToValidate="txtWorkgroups" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Workgroup -"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                            <span style="position: relative;float:left;">
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtWorkgroupForAssignImage"
                                                    PopupControlID="Panel4" OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="Panel4" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="CBWorkgroupForAssignImage" runat="server" Style="font-size: 100%;
                                                        font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Product Category</span> <span class="select-drop medium-drop">
                                                    <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- Product Category -</asp:ListItem>
                                                    </asp:DropDownList>
                                        <%--            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Select Placement"
                                                        Display="None" CssClass="error" ControlToValidate="ddlProductCategory" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft" runat="server" id="DivProductFolder">
                                            <label>
                                                <span class="lbl-txt">Folder</span> <span class="select-drop medium-drop">
                                                    <asp:DropDownList ID="ddlFolderForAssignImage" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Select Placement"
                                                        Display="None" CssClass="error" ControlToValidate="ddlFolderForAssignImage" ValidationGroup="MediaAssignedToStore"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <div style="clear: both;">
                                    </div>
                                    
                                    <div class="emp-btn-block">
                                        <a id="A6" href="javascript:;" tabindex="20" class="cancel-popup gray-btn">Cancel</a>
                                        <asp:Button ID="BtnAssignToStore" runat="server" CssClass="blue-btn submit" ValidationGroup="MediaAssignedToStore"
                                            call="MediaAssignedToStore" Text="Save" OnClick="BtnAssignToStore_Click1" />
                                        <asp:HiddenField ID="HfStyleID" runat="server" Value="0" />
                                        <asp:HiddenField ID="HfStoreImage" runat="server" Value="" />
                                        <asp:HiddenField ID="HFStoreImageMode" runat="server" Value="0" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Add Media Popup --%>
        <%-- Start Help Video Popup --%>
        <div class="popup-outer" id="video-block">
            <div class="popupInner">
                <div class="video-block">
                    <a href="javascript:;" class="hide-popup" onclick="CloseMediaHelpVideo('ctl00_ContentPlaceHolder1_iframeVideo')">Close</a>
                    <div class="video-player" style="overflow: hidden; width: 874px; height: 517px">
                        <iframe id="iframeVideo" runat="server" width="874" height="517" frameborder="0">
                        </iframe>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Help Video Popup --%>
    </div>
</asp:Content>
