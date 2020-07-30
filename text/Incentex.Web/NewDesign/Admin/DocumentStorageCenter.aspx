<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="DocumentStorageCenter.aspx.cs" EnableEventValidation="false" Inherits="NewDesign_Admin_DocumentStorageCenter"
    Title="incentex | Document & Media Storage Management" %>

<%--<%@ Register TagPrefix="ucMultidrp" TagName="MultiSelectDropBox" Src="~/usercontrol/MultiselectDropbox.ascx" %>--%>
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
        function DeleteConfirmation(itemname) {
            if (confirm("Are you sure, you want to delete this "+ item +" ?") == true)
                return true;
            else
                return false;
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
        
    
    $(document).ready(function (){
    
    
        $(window).ValidationUI();
        $("#ctl00_ContentPlaceHolder1_txtWorkgroups").attr("autocomplete", "off");
        $("#ctl00_ContentPlaceHolder1_txtCompany").attr("autocomplete", "off");
        
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
        
        
    function Success(FileType,FileName) {
   
   
     var filetype= FileType.substring(1,6);
     
    if(filetype=="image")
    {
            
            document.getElementById("ctl00_ContentPlaceHolder1_imgProductImage").src="../DocmentStorageCenter/" + FileName;
            document.getElementById("ctl00_ContentPlaceHolder1_HfStoreImage").value="../DocmentStorageCenter/" + FileName;
            document.getElementById("ctl00_ContentPlaceHolder1_HFStoreImageMode").value="Add";
            
            $("#ctl00_ContentPlaceHolder1_ddlProductCategory").val("0");
            $("#ctl00_ContentPlaceHolder1_ddlProductCategory").prev("span").html("- Product Category -");
            $("#ctl00_ContentPlaceHolder1_ddlFolderForAssignImage").val(0);
            $("#ctl00_ContentPlaceHolder1_ddlFolderForAssignImage").prev("span").html("- Folder -");
            $("#ctl00_ContentPlaceHolder1_txtMasterStyleName").val("");
            
            $("[id*=cbCompanyforassignimage]").attr("onclick", "ValidationForMultiSelection(this,'ctl00_ContentPlaceHolder1_txtCompanyforassignimage','ctl00_ContentPlaceHolder1_cbCompanyforassignimage','- Company -')");
            document.getElementById("ctl00_ContentPlaceHolder1_txtCompanyforassignimage").value="- Company -";
            $('#ctl00_ContentPlaceHolder1_cbCompanyforassignimage input').each(function () {
            $(this).parent().attr('class', 'icheckbox_flat');
            $(this).attr('checked', false);
            });
             
            $("[id*=CBWorkgroupForAssignImage]").attr("onclick", "ValidationForMultiSelection(this,'ctl00_ContentPlaceHolder1_txtWorkgroupForAssignImage','ctl00_ContentPlaceHolder1_CBWorkgroupForAssignImage','- Workgroup -')");
            document.getElementById("ctl00_ContentPlaceHolder1_txtWorkgroupForAssignImage").value="- Workgroup -";
            
            $('#ctl00_ContentPlaceHolder1_CBWorkgroupForAssignImage input').each(function () {
            $(this).parent().attr('class', 'icheckbox_flat');
            $(this).attr('checked', false);
            });
            
            ShowPopUp("add-assigntostoreproduct","employess-content");
            
            }
    else
    {
          ShowPopUp("add-file-block","emp-content");
    }

    }
    
  


 
    function Error() {
         //document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "Upload failed. Please Try Again.";
    }</script>

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
        $(".youtube").colorbox({ iframe: true, innerWidth: 640, innerHeight: 390 });
        $(".vimeo").colorbox({ iframe: true, innerWidth: 500, innerHeight: 409 });
        $(".iframe").colorbox({ iframe: true, width: "80%", height: "80%" });
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

    <style>
        /*#media-div-popup .CompletionListCssClass{
            font-family: 'AvenirLTStd-Black';
            font-size: 13px;
            color: #929292;
            border: 1px solid #999;
            background: white;
            margin-left: 0px;
            
            height: 150px;
            overflow:auto;
        }
        #media-div-popup .CompletionListCssClassSelected
        {
            font-size: 13px;
            color: white;
            border: 1px solid #999;
            background: #3065D0;
            width: 260px;
            margin-left: 0px;
        }*/
       /* #media-div-search .CompletionListCssClass
        {
            border: 1px solid #E4E4E4;
            border-radius: 4px;
            color: #929292;
            font-family: 'AvenirLTStd-Black';
            font-size: 14px;
            height: 27px;
            vertical-align: middle;
            font-size: 13px;
            padding: 3px 5px;
            border: 1px solid #999;
            background: #fff;
            width: 600px;
            float: left;
            position: absolute;
            margin-left: 0px;
            height: 150px;
            overflow: auto;
        }
        #media-div-search .CompletionListCssClassSelected
        {
            font-size: 13px;
            color: white;
            border: 1px solid #999;
            background: #3065D0;
            width: 600px;
            float: left;
            margin-left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <asp:HiddenField ID="hdnVideoLink" runat="server" />
    <asp:HiddenField ID="hdnBasicUserInfoID" runat="server" />
    <asp:Button ID="btnClearEmployeeDetails" runat="server" Text="Clear Employee Details"
        Style="display: none;" />
    <% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% } %>
    <div id="container" class="cf filter-page">
    <div id="media-div" style="width:100%">
    <div class="widecolumn alignright" style="width: 100%" id="DocumentStoragecenter">
            <div class="filter-content">
                <div class="filter-header cf" >
                    <span class="title-txt">Document and Media Storage</span> <em id="totalcount_em"
                        runat="server" visible="false"></em>
                    <div class="filter-search top">
                        <div class="title-txt">
                            <%--<asp:LinkButton ID="lbHelpVideo" runat ="server" Text="Help Video" 
                                onclick="lbHelpVideo_Click"></asp:LinkButton>--%>
                                <a href="javascript:;" title="Help video" onclick="GetHelpVideo('Document and Media Storage','Document and Media Storage')">Help video</a>
                                <%--<div title="Help video" onclick="GetHelpVideo('Document and Media Storage','Document and Media Storage')">Help video</div>--%>
                            </div>
                            
                    </div>
                </div>
                <div class="filter-tableblock cf">
                </div>
            </div>
            <div class="filter-content">
            <div class="filter-header cf" class="MediaFilter">
                    <span class="medfile" >MEDIA FILES</span>
                    <div class="filter-search">
                        <asp:DropDownList ID="ddlSearchcomp" runat="server" CssClass="SearchDropDown default">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlSearchWorkgroup" runat="server" CssClass="SearchDropDown default">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="input-field-small default_title_text"
                            ToolTip="Search ..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" TargetControlID="txtSearch"
                                                runat="server" ServiceMethod="GetMediaSearchKeywords" ServicePath="~/NewDesign/UserPages/WSUser.asmx"
                                                MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="true" CompletionListCssClass="CompletionListCssClassSearchTags"
                                                CompletionListItemCssClass="CompletionListItemCssClassSearchTags" CompletionListHighlightedItemCssClass="CompletionListCssClassSelectedSearchTags" />
                        <asp:LinkButton ID="btnGo" runat="server" CssClass="go-btn" Enabled="true" 
                             onclick="btnGo_Click">
                            GO</asp:LinkButton>
                    </div>
                </div>
               
                <div class="filter-tableblock cf">
                </div>
            </div>
            <br />
            <div class="upload-form1" style="height:120px;border:solid 1px #D3D3D3">
            <input type="hidden" id="HFdragvalue" value="Doc"  />
            <iframe src="../DragDrop.aspx" height="120px" width="100%" scrolling="no" ></iframe> 
             <%--  <asp:AjaxFileUpload ID="AjaxFileUpload1" runat="server" OnClientUploadComplete="Success"
                    ThrobberID="loader" Width="100%" MaximumNumberOfFiles="1" OnUploadComplete="UploadComplete" />
                <asp:Image ID="loader" runat="server" ImageUrl="../../Images/date-bg.gif" Style="display: None" />--%>
                <br />
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <br /> <br />
        </div>
       
        <div class="FolderName" id="divshowfoldername" runat ="server"  >  <asp:Literal ID="ltShowFolderName" runat ="server"></asp:Literal> </div> 
        <div class="BackButton">
                      <div class="title-txt title-back"  id="btnBack" runat ="server" visible ="false"  >
                        <a title="Back" href="documentStorageCenter.aspx">Back</a>
                      </div></div>
            <table cellpadding="0" cellspacing="0" style="border:solid 1px #D3D3D3; width:100%;">
            <tr>
            <td align="center" style="color:#929292;font-weight:bold;font-family: AvenirLTStd-Medium;font-size: 18px;">
            <asp:Label ID="lblErrorMessage" runat ="server" ></asp:Label> 
            
            </td>
            </tr>
          <tr>
          <td>
          <div style="padding-top:0px;padding-bottom:0px;padding-left:14px;">
          
      <%--  <asp:UpdatePanel ID="upd1" runat ="server">
        <ContentTemplate>--%>
          <asp:DataList ID="dlMediaFiles" runat ="server" RepeatColumns="2" 
                  RepeatDirection="Vertical" Width="100%" 
                  onitemcommand="dlMediaFiles_ItemCommand" 
                  OnItemDataBound="dlMediaFiles_ItemDataBound">
          <ItemTemplate>
         <table cellpadding="">
         <tr>
         <td id="myrecord" runat ="server">
         <div class="vidtag" >
            <div class="imagediv imgbox-shadow">
            <%# GetMediaImage(Eval("OriginalFileName").ToString(), Eval("filetype").ToString(), Eval("name").ToString(), Eval("id").ToString())%></div>
            <div class="vidname">
                          <span class="dmsmainlist-title"><%# Eval("name")%></span>
                          <span class="dmsmainlist-views"><%# Eval("createddate") %>
                           <br />
                          <%# Eval("view") == null ? 0 : Eval("view")%> Views</span> </div>
                            </div>
                            <div class="vidname-btn" id="divMybuttons" runat ="server" visible ='<%#CheckForSystemFolder(Eval("filetype").ToString(),Eval("id").ToString()) %>'>
                                    <asp:Button ID="btnAssingedToStore"  runat ="server" CssClass="dms-btn" Text="Assign To Store" ToolTip="ASSIGN TO STORE"  Visible='<%#ShowAssignedToStore(Eval("filetype").ToString(),"assign to store") %>' CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' OnClientClick="ShowDefaultLoader();"  /> 
                                    <asp:Button ID="btnAssignMedia"  runat ="server" CssClass="dms-btn" Text="ASSIGN MEDIA" ToolTip="ASSIGN MEDIA"  Visible='<%#ShowAssigned(Eval("filetype").ToString()) %>' CommandName='<%# "asg-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' OnClientClick="ShowDefaultLoader();"  /> 
                                     <asp:Button ID="btnPublished" runat ="server" CssClass="dms-btn" Text="PUBLISH" ToolTip="PUBLISH" Visible='<%#ShowAssigned(Eval("filetype").ToString()) %>' CommandName='<%# "pub-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' OnClientClick="ShowDefaultLoader();" /> 
                                     <asp:Button ID="btnMoveFile" runat ="server" CssClass="dms-btn"  Text="MOVE FILE" ToolTip="MOVE FILE" CommandName='<%# "mov-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' visible ='<%#ShowForFolder(Eval("filetype").ToString()) %>'   />
                                     <asp:Button ID="btnRename"  runat ="server" CssClass="dms-btn" Text ="RENAME" ToolTip="RENAME" CommandName='<%# "ren-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' OnClientClick="ShowDefaultLoader();" Visible='<%#ShowAssignedToStore(Eval("filetype").ToString(),"rename") %>' /> 
                                     <asp:Button ID="btnRemove" runat ="server" CssClass="dms-btn media-del-button"  Text="REMOVE" ToolTip="REMOVE" CommandName='<%# "del-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>'  /> 
                                     <asp:HiddenField ID="hfassignid" runat ="server" /> 
                                     <asp:HiddenField ID="hfassignvideo" runat ="server" /> 
                                     <asp:HiddenField ID="hfFileType" runat ="server" Value='<%# Eval("filetype") %>' /> 
                            </div>
                            <div class="vidname-btn" id="div1" runat ="server" visible ='<%#ShowRemoveButtonforSystemFolder(Eval("filetype").ToString(),Eval("id").ToString()) %>'  >
                            <asp:Button ID="btnMoveForSystemFolder" runat ="server" CssClass="dms-btn"  Text="MOVE FILE" ToolTip="MOVE FILE" CommandName='<%# "mov-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' visible ='<%#ShowForFolder(Eval("filetype").ToString()) %>'   />
                            <asp:Button ID="btnRenameForSysfolder"  runat ="server" CssClass="dms-btn" Text ="RENAME" ToolTip="RENAME" CommandName='<%# "ren-" + Eval("filetype").ToString() %>' CommandArgument='<%#Eval("id").ToString() %>' OnClientClick="ShowDefaultLoader();" Visible='<%#ShowAssignedToStore(Eval("filetype").ToString(),"rename") %>' /> 
                            
                            </div>
                            
                             
          </div>
          </td>
         </tr>
         <tr>
         <td>
             &nbsp;
         </td>
         </tr>
              </table>
             </ItemTemplate>
          </asp:DataList>
        <%--  </ContentTemplate>
        </asp:UpdatePanel> --%>
        </div>
        <br />
        <br />
       <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" CssClass="pagination alignright view-link postback cf" OnClientClick="ShowDefaultLoader();">
                        VIEW ALL
                    </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPrevious_Click" ToolTip="Previous" OnClientClick="ShowDefaultLoader();">
                            </asp:LinkButton>
                        </span>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPaging" runat="server" CommandArgument='<%# Eval("Index") %>'
                                    CommandName="ChangePage" Text='<%# Eval("Index") %>' CssClass="postback" OnClientClick="ShowDefaultLoader();">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkNext" CssClass="right-arrow alignright postback" runat="server"
                            OnClick="lnkNext_Click" ToolTip="Next" OnClientClick="ShowDefaultLoader();" >
                        </asp:LinkButton>
                    </div>
                </div>
              </td>
          </tr>
          <tr>
          <td>
              &nbsp;
          </td>
          </tr>
          </table>
          </div>
    </div> 
        <% if (!Request.IsLocal)
           { %>
    </section>
    <%}
    %>
    <div id="media-div-popup">
        <%-- Start Add Media Popup --%>
        <div id="add-media-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Assign Media')">
                    Help Video</a>
                <%--<a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetDocumentLink('Employee Uniform','Ramp Agent Uniforms')">Help Video</a>--%>
                <a href="javascript:;" class="close-btn" id="aCloseAddEmployee">Close</a>
                <div class="employee-pop-block">
                    <div class="employess-content" style="border: 0px;">
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
                                                    <%--<img src="../../Images/vidimage1.png" width="144px" height="144px" />--%>
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
                                                    <%--<img src="../../Images/PageThumbNail.png" width="144px" height="144px" />--%>
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
                                                    <%--  <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Select Document"
                                                        Display="None" CssClass="error" ControlToValidate="txtCompany" ValidationGroup="MediaAssigned"
                                                        SetFocusOnError="True" InitialValue="- Company -"></asp:RequiredFieldValidator>--%>
                                                </span>
                                            </label>
                                            <span style="position: relative; float: left">
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
                                            <span style="position: relative; float: left">
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtWorkgroups" PopupControlID="Panel2"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="Panel2" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="cbWorkGroup" runat="server" Style="font-size: 100%; font-family: Sans-Serif;">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </span></li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Placement</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
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
                                            <span style="position: relative; float: left">
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" DynamicServicePath=""
                                                    Enabled="True" ExtenderControlID="" TargetControlID="txtModuleForDoc" PopupControlID="PanalForModuleDoc"
                                                    OffsetY="22">
                                                </asp:PopupControlExtender>
                                                <asp:Panel ID="PanalForModuleDoc" runat="server" Height="116px" Width="275px" BorderStyle="Solid"
                                                    BorderWidth="1px" Direction="LeftToRight" ScrollBars="Auto" BackColor="#F5F5F5"
                                                    Style="display: none; margin-top: 27px; border-top: none;" BorderColor="Black">
                                                    <asp:CheckBoxList ID="CbModuleForDoc" runat="server" Style="font-size: 100%; font-family: Sans-Serif;"
                                                        AutoPostBack="true" OnSelectedIndexChanged="CbModuleForDoc_SelectedIndexChanged"
                                                        onchange="ShowDefaultLoader();">
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
                                            <span style="position: relative; float: left">
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
                                            <span style="position: relative; float: left;">
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
                                            <span style="position: relative; float: left">
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
                                            <span style="position: relative; float: left;">
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
                                                <span class="lbl-txt">Search Tags</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtMediaSearchTag" runat="server" class="input-field-all checkvalidation"
                                                    TabIndex="15" MaxLength="500"></asp:TextBox>
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
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
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
                                        <asp:LinkButton ID="lbAssignedMedia" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                            ValidationGroup="MediaAssigned" OnClick="lbAssignedMedia_Click" Visible="false">
                                        </asp:LinkButton>
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
                                    <hr style="border: solid 1px #E2E2E2; width: 98%;" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Master Item #</span> <span class="select-drop medium-drop">
                                                    <asp:TextBox ID="txtMasterStyleName" runat="server" class="input-field-all checkvalidation autotext"
                                                        TabIndex="15" MaxLength="200"></asp:TextBox>
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
                                            <span style="position: relative; float: left;">
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
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
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
                                            <span style="position: relative; float: left;">
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
                                    <div style="clear: both; display: none;">
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                    <div class="emp-btn-block">
                                        <a id="A6" href="javascript:;" tabindex="20" class="cancel-popup gray-btn">Cancel</a>
                                        <asp:Button ID="BtnAssignToStore" runat="server" CssClass="blue-btn submit" ValidationGroup="MediaAssignedToStore"
                                            call="MediaAssignedToStore" Text="Save" OnClick="BtnAssignToStore_Click1" />
                                        <asp:HiddenField ID="HfStyleID" runat="server" Value="0" />
                                        <asp:HiddenField ID="HfStoreImage" runat="server" Value="" />
                                        <asp:HiddenField ID="HFStoreImageMode" runat="server" Value="0" />
                                        <%--<asp:LinkButton ID="lbAssignToStore" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                        ValidationGroup="MediaAssignedToStore" OnClick="lbAssignedMedia_Click" Visible="false">
                                    </asp:LinkButton>--%>
                                        <%--<asp:HiddenField ID="" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfMediaAssignedid" runat="server" Value="0" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Add Media Popup --%>
        <div class="popup-outer" id="employee-Details-Block" style="display: none;">
            <div class="popupInner">
                <div class="message-popup employee-pop-block">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);">Help Videoript:;&quot;
                        class=&quot;close-btn&quot; id=&quot;aCloseEmployeeDetails&quot;&gt;Closent emp-height&quot;&gt;
                        <li class="active" tab-id="setup"><a href="javascript:;" title="Setup"><em></em>Setup</a></li>
                        <li tab-id="products"><a href="javascript:;" title="Products"><em></em>Products</a></li>
                        <li tab-id="preview"><a href="javascript:;" title="Admin Settings"><em></em>Preview</a></li>
                        <li class="last" tab-id="history"><a href="javascript:;" title="History"><em></em>History</a></li>
                        </ul>
                        <div class="employee-form current-tab Setup">
                        </div>
                        <div class="employee-form current-tab products" style="display: none;">
                        </div>
                        <div class="employee-form current-tab preview" style="display: none;">
                        </div>
                        <div class="employee-form current-tab history" style="display: none;">
                        </div>
                </div>
            </div>
        </div>
        <%-- Start Add File Popup --%>
        <div id="add-file-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <div class="employee-pop-block">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Add Media File')">
                        Help Video</a><a href="javascript:;" class="close-btn" id="a1">Close</a>
                    <div class="emp-content media-div-file-height">
                        <h2>
                            Add Media File</h2>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Name File</span>
                                                <div class="date-field">
                                                    <asp:TextBox ID="txtNameFile" runat="server" class="input-field-all first-field checkvalidation"
                                                        TabIndex="15" MaxLength="200"></asp:TextBox>
                                                    <asp:HiddenField ID="hfShowimage" runat="server" Value="0" />
                                                    <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ErrorMessage="Please enter file name"
                                                        Display="None" CssClass="error" ControlToValidate="txtNameFile" ValidationGroup="MediaFile"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <div id="showautoextender">
                                                    </div>
                                                </div>
                                            </label>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Search Tags</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtSearchTag" runat="server" class="input-field-all checkvalidation"
                                                    TabIndex="15" MaxLength="500"></asp:TextBox>
                                            </div>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Private Folder</span>
                                            </label>
                                            <%--  <asp:DropDownList ID="DropDownList3" runat="server" CssClass="default checkvalidation">
                                                <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                            </asp:DropDownList>--%>
                                            <span class="select-drop warranty-mid-drop popup" id="warrantyby_parent_span" runat="server"
                                                data-content="add-file-block;add-file-block .emp-content">
                                                <cdd:CustomDropDown ID="ddlFoldeName" runat="server" DropDownCssClass="default checkvalidation"
                                                    TextBoxCssClass="input-field-all" ParentSpanClassToRemove="select-drop warranty-mid-drop popup"
                                                    Module="mediafolder" ToolTip="Folder" OnSelectedIndexChanged="ddlCommonDropDown_SelectedIndexChanged"
                                                    OnSaveNewOptionAttempted="ddlLookUpDropDown_SaveNewOptionAttempted" />
                                            </span></li>
                                        <li class="alignleft" id="FolderPassword" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Password</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="input-field-all checkvalidation"
                                                    TabIndex="15" MaxLength="200"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please enter file name"
                                                    Display="None" CssClass="error" ControlToValidate="txtPassword" ValidationGroup="MediaFile"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </div>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <div class="emp-btn-block">
                                        <%--<a id="A2" href="javascript:;" tabindex="20" class="gray-btn">Cancel</a>--%>
                                        <asp:LinkButton ID="lnkCancel" runat="server" CssClass="gray-btn ">Cancel</asp:LinkButton>
                                        <asp:LinkButton ID="btnSave" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                            ValidationGroup="MediaFile" call="MediaFile" OnClick="btnSave_Click">
                                        Save
                                        </asp:LinkButton>
                                        <asp:Button ID="btnCancel" ValidationGroup="MediaFile" runat="server" Visible="false" />
                                        <asp:HiddenField ID="HfMediaFileName" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Add Media Popup --%>
        <%-- Start Rename Popup --%>
        <div id="add-rename-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <div class="employee-pop-block" style="height: 519px">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Rename')">
                        Help Video</a><a href="javascript:;" class="close-btn" id="a2">Close</a>
                    <div class="renamefile-content media-div-file-height">
                        <h2>
                            Rename</h2>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">
                                                    <asp:Literal ID="ltRenameFile" runat="server"></asp:Literal></span>
                                                <div class="date-field">
                                                    <asp:TextBox ID="txtRenameFileName" runat="server" class="input-field-all first-field checkvalidation"
                                                        TabIndex="15" MaxLength="200"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfRenameFileName" runat="server" ErrorMessage="Please enter file name"
                                                        Display="None" CssClass="error" ControlToValidate="txtRenameFileName" ValidationGroup="RenameMediaFile"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:HiddenField ID="hfRenfileid" runat="server" />
                                                    <asp:HiddenField ID="hfRenfileType" runat="server" />
                                                    <asp:HiddenField ID="hfRenamePassword" runat="server" />
                                                    <%--<asp:CustomValidator ID="cvFileNameExist" runat="server" ErrorMessage="File name already exists"
                                                    ValidationGroup="MediaFile" ControlToValidate="txtFileName" CssClass="error"
                                                    Display="Dynamic" SetFocusOnError="True" ClientValidationFunction="CheckNameAlreadyExists"></asp:CustomValidator>--%>
                                                </div>
                                            </label>
                                        </li>
                                        <li class="alignleft" id="liRenamePassord" runat="server" visible="false">
                                            <label>
                                                <span class="lbl-txt">Password</span>
                                            </label>
                                            <div class="date-field">
                                                <asp:TextBox ID="txtRenamePassword" TextMode="Password" runat="server" class="input-field-all"
                                                    TabIndex="15" MaxLength="200"></asp:TextBox>
                                            </div>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <div class="emp-btn-block">
                                        <%--<a id="A2" href="javascript:;" tabindex="20" class="gray-btn">Cancel</a>--%>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="gray-btn">Cancel</asp:LinkButton>
                                        <asp:LinkButton ID="btnRenameFileSave" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                            ValidationGroup="RenameMediaFile" call="RenameMediaFile" OnClick="btnRenameFileSave_Click">
                                        Save
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Rename Popup --%>
        <%-- Start Rename Popup --%>
        <div id="add-remove-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <div class="employee-pop-block" style="height: 519px;">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Remove')">
                        Help Video</a><a href="javascript:;" class="close-btn" id="a5">Close</a>
                    <div class="removefile-content media-div-file-height">
                        <h2>
                            Remove</h2>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt" style="width: 130px;">Folder Password:</span>
                                                <div class="date-field">
                                                    <asp:TextBox ID="txtremovePassword" runat="server" class="input-field-all" TabIndex="15"
                                                        MaxLength="200" TextMode="Password"></asp:TextBox>
                                                    <asp:HiddenField ID="hfremovefolderid" runat="server" />
                                                    <asp:HiddenField ID="hfRemovepassword" runat="server" />
                                                    <asp:HiddenField ID="hfRemovefileid" runat="server" />
                                                </div>
                                            </label>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <div class="emp-btn-block">
                                        <asp:LinkButton ID="LinkButton4" runat="server" CssClass="gray-btn">Cancel</asp:LinkButton>
                                        <asp:LinkButton ID="btnRemoveSave" runat="server" TabIndex="21" CssClass="blue-btn submit"
                                            OnClick="btnRemoveSave_Click">
                                        Remove
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Rename Popup --%>
        <%-- Start Help Video Popup --%>
        <div class="popup-outer" id="video-block">
            <div class="popupInner">
                <div class="video-block">
                    <a href="javascript:;" class="hide-popup" onclick="CloseMediaHelpVideo('ctl00_ContentPlaceHolder1_iframeVideo')">
                        Close</a>
                    <div class="video-player" style="overflow: hidden; width: 874px; height: 517px">
                        <iframe id="iframeVideo" width="874" height="517" frameborder="0" runat="server">
                        </iframe>
                    </div>
                </div>
            </div>
        </div>
        <%-- End Help Video Popup --%>
        <%-- Start View Publish video Popup --%>
        <div id="add-publish-block" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Publish Media')">
                    Help Video</a><a href="javascript:;" class="close-btn" id="a3" onclick="CloseMediaHelpVideo('ctl00_ContentPlaceHolder1_iframepubvideo')">Close</a>
                <div class="employee-pop-block" style="height: 519px;">
                    <div class="dmsvideo-content emp-content emp-height">
                        <h2>
                            Publish Media</h2>
                        <div class="videobox-shadow" id="divvidimage" runat="server" visible="false">
                            <%--<a id="AVidTag" title="Play" class="videoplay-icon" href="#" runat="server">Play</a>--%>
                            <asp:LinkButton ID="lbPlayPubVideo" CssClass="videoplay-icon" runat="server" OnClick="lbPlayPubVideo_Click">Play</asp:LinkButton>
                            <asp:Literal ID="ltPubvideotag" runat="server"></asp:Literal>
                        </div>
                        <div class="videobox-shadow" runat="server" visible="false" id="divShowPubVideo">
                            <iframe id="iframepubvideo" runat="server" width="462px" height="344px" frameborder="0"
                                visible="false"></iframe>
                        </div>
                        <div class="dmsvideo-publishtext" style="display: block">
                            <div>
                                <div class="basic-form cf">
                                    <div>
                                        <ul class="cf publishtext-list">
                                            <li class="alignleft" style="width: 350px;">
                                                <label>
                                                    <span style="color: Red; float: left;">
                                                        <asp:Label ID="lblPubInstruction" runat="server"></asp:Label></span>
                                                </label>
                                            </li>
                                        </ul>
                                        <h3>
                                            <asp:Literal ID="ltPublishedFileName" runat="server"></asp:Literal></h3>
                                        <%-- <ul class="cf publishtext-list" style="width: 834px; display: none;">
                                            <li class="alignleft" style="width: 399px;">
                                                <label>
                                                    <span class="lbl-txt"><span style="padding-right: 48px;">Company:</span>
                                                        <asp:Literal ID="ltpublishedCompany" runat="server"></asp:Literal>
                                                        <br />
                                                        <span style="padding-right: 36px;">Workgroup:</span>
                                                        <asp:Literal ID="ltPublishWorkgroup" runat="server"></asp:Literal><br />
                                                        <span style="padding-right: 48px;">Published:</span>
                                                        <asp:Literal ID="ltPublishdates" runat="server"></asp:Literal>
                                                       
                                                        <div id="divVimeoUrl" runat="server">
                                                            <span class="lbl-txt">Vimeo URL:
                                                                <asp:Literal ID="ltVimeoUrl" runat="server"></asp:Literal></span>
                                                        </div>
                                                    </span>
                                                </label>
                                            </li>
                                           
                                            <li class="alignright" style="width: 418px;">
                                                <label>
                                                    <span class="lbl-txt" style="width: 255px"><span style="padding-right: 36px;">Placement:</span>
                                                        <asp:Literal ID="ltPublishPlacement" runat="server"></asp:Literal><br />
                                                        <span style="padding-right: 59px;">Module:</span>
                                                        <asp:Literal ID="ltPublishlocation" runat="server"></asp:Literal><br />
                                                        <span style="padding-right: 75px;">Place:</span>
                                                        <asp:Literal ID="ltPublishPlace" runat="server"></asp:Literal><br />
                                                       </span>
                                                </label>
                                            </li>
                                        </ul>--%>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" id="tdfiledetail">
                                                    <ul class="left-form cf publishtext-list" style="border-bottom: none; width: 417px;">
                                                        <li class="alignleft" style="width: 411px;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Company:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltpublishedCompany" runat="server"></asp:Literal>
                                                                    <asp:HiddenField runat="server" ID="hfPubVideo" />
                                                                </span>
                                                            </label>
                                                        </li>
                                                        <li class="alignleft" style="width: 411px; clear: both;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Workgroup:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltPublishWorkgroup" runat="server"></asp:Literal>
                                                                </span>
                                                            </label>
                                                        </li>
                                                        <li class="alignleft" style="width: 411px; clear: both;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Published:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltPublishdates" runat="server"></asp:Literal>
                                                                </span>
                                                            </label>
                                                        </li>
                                                        <li class="alignleft" style="width: 411px; clear: both;" id="divVimeoUrl" runat="server">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Vimeo URL:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px; word-wrap: break-word;">
                                                                    <asp:Literal ID="ltVimeoUrl" runat="server"></asp:Literal>
                                                                </span>
                                                            </label>
                                                        </li>
                                                    </ul>
                                                </td>
                                                <td valign="top" id="tdfiledetail">
                                                    <ul class="left-form cf publishtext-list" style="border-bottom: none; width: 407px;">
                                                        <li class="alignright" style="width: 418px;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Placement:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltPublishPlacement" runat="server"></asp:Literal></span>
                                                            </label>
                                                        </li>
                                                        <li class="alignright" style="width: 418px;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Module:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltPublishlocation" runat="server"></asp:Literal>
                                                                </span>
                                                            </label>
                                                        </li>
                                                        <li class="alignright" style="width: 418px;">
                                                            <label>
                                                                <span class="lbl-txt-feild" style="width: 104px">Place:</span> <span class="lbl-txt-feild"
                                                                    style="width: 278px">
                                                                    <asp:Literal ID="ltPublishPlace" runat="server"></asp:Literal>
                                                                </span>
                                                            </label>
                                                        </li>
                                                    </ul>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="">
                                        </div>
                                        <ul class="left-form cf" style="border-bottom: none; width: 834px;" id="ulPubfolder"
                                            runat="server">
                                            <li class="alignleft" runat="server" id="Li2">
                                                <label>
                                                    <span class="lbl-txt" style="width: 115px;">Folder</span> <span class="select-d">
                                                    </span><span class="select-drop medium-drop">
                                                        <asp:DropDownList ID="ddlPubFolder" runat="server" CssClass="default">
                                                            <asp:ListItem Value="0" Selected="True">- Folder -</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </span>
                                                </label>
                                            </li>
                                            <li class="alignleft" runat="server" id="Li3" runat="server">
                                                <label>
                                                    <span class="lbl-txt" style="width: 115px;">Folder Password</span>
                                                </label>
                                                <div class="date-field">
                                                    <asp:TextBox ID="txtPubPassword" TextMode="Password" runat="server" class="input-field-all"
                                                        TabIndex="15" MaxLength="200"></asp:TextBox>
                                                    <asp:HiddenField ID="hfPubPassword" runat="server" />
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="emp-btn-block">
                                        <%--<a id="A4" href="javascript:;" class="gray-btn"></a>--%>
                                        <asp:LinkButton ID="lbContWithoutPublish" runat="server" class="gray-btn" OnClick="lbContWithoutPublish_Click">Continue without Publishing</asp:LinkButton>
                                        <asp:LinkButton ID="lbPublishvideo" runat="server" CssClass="blue-btn submit" OnClick="lbPublishvideo_Click1"> 
                                    Publish to Vimeo
                                        </asp:LinkButton>
                                        <asp:HiddenField ID="HfPubMediaid" runat="server" Value="0" />
                                        <asp:HiddenField ID="HfPubAssgnId" runat="server" Value="0" />
                                        <asp:HiddenField ID="HFPubType" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- End View Publish video Popup --%>
        <div id="move-file" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <div class="employee-pop-block" style="height: 519px;">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Move File')">
                        Help Video</a><a href="javascript:;" class="close-btn" id="a7">Close</a>
                    <div class="removefile-content media-div-file-height">
                        <h2>
                            Move File</h2>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Select File</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlMoveFile" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- File -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Select Placement"
                                                        Display="None" CssClass="error" ControlToValidate="ddlMoveFile" ValidationGroup="FileMove"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Current Folder</span> <span class="select-drop medium-drop"
                                                    style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlMoveCurrentFolder" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;" Enabled="false">
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Folder To Move</span> <span class="select-drop medium-drop"
                                                    style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlFolderToMove" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Select Placement"
                                                        Display="None" CssClass="error" ControlToValidate="ddlFolderToMove" ValidationGroup="FileMove"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <div class="emp-btn-block">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="gray-btn">Cancel</asp:LinkButton>
                                        <%--<asp:LinkButton ID="btnMove" runat="server" TabIndex="21" CssClass="blue-btn submit" ValidationGroup="FileMove"
                                            >
                                        Move
                                        </asp:LinkButton>"--%>
                                        <asp:Button ID="btnMove" runat="server" CssClass="blue-btn submit" ValidationGroup="FileMove"
                                            call="FileMove" Visible="true" Text="Move" OnClick="btnMove_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="move-image" class="popup-outer">
            <!--" -->
            <div class="popupInner">
                <div class="employee-pop-block" style="height: 519px;">
                    <a class="help-video-btn" title="Help Video" href="javascript: void(0);" onclick="GetHelpVideo('Document and Media Storage','Move File')">
                        Help Video</a><a href="javascript:;" class="close-btn" id="a8">Close</a>
                    <div class="removefile-content media-div-file-height">
                        <h2>
                            Move File</h2>
                        <div class="employee-form current-tab manualentry tabcon" style="display: block;">
                            <div>
                                <div class="basic-form cf">
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Select File</span> <span class="select-drop medium-drop" style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlMoveImageFile" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- File -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Select File"
                                                        Display="None" CssClass="error" ControlToValidate="ddlMoveImageFile" ValidationGroup="ImageMove"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <hr style="border: solid 1px #E2E2E2; width: 98%" />
                                    <ul class="left-form cf" style="border-bottom: none;">
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Current Folder</span> <span class="select-drop medium-drop"
                                                    style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlMoveCurrentImageFolder" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;" Enabled="false">
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                            </label>
                                        </li>
                                        <li class="alignleft">
                                            <label>
                                                <span class="lbl-txt">Folder To Move</span> <span class="select-drop medium-drop"
                                                    style="margin-top: 3px;">
                                                    <asp:DropDownList ID="ddlFolderToMoveImage" runat="server" CssClass="default checkvalidation"
                                                        Style="border: solid 2px red;">
                                                        <asp:ListItem Selected="True">- Folder -</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Select Folder"
                                                        Display="None" CssClass="error" ControlToValidate="ddlFolderToMoveImage" ValidationGroup="ImageMove"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                        </li>
                                    </ul>
                                    <div class="emp-btn-block">
                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="gray-btn">Cancel</asp:LinkButton>
                                        <%--<asp:LinkButton ID="btnMove" runat="server" TabIndex="21" CssClass="blue-btn submit" ValidationGroup="FileMove"
                                            >
                                        Move
                                        </asp:LinkButton>"--%>
                                        <asp:Button ID="Button1" runat="server" CssClass="blue-btn submit" ValidationGroup="ImageMove"
                                            call="ImageMove" Visible="true" Text="Move" OnClick="btnMoveImage_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
