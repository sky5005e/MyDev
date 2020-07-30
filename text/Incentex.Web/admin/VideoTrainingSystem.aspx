<%@ Page Title="Video Training System" Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="VideoTrainingSystem.aspx.cs" Inherits="admin_VideoTrainingSystem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/usercontrol/TrainingVideo.ascx" TagName="VideoUserControl" TagPrefix="vti"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
        assigndesign();
    }
    </script>
    <script type="text/javascript" language="javascript">

        var formats = 'mp4|mov';

        $().ready(function() {
            $.get('../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);

                $.validator.addMethod("filesize", function(value, element, param) {
                    var size = element.files[0].size;
                    if ((size / 1048576) > param)
                        return false;
                    else
                        return true;
                }, "The file you are uploading is more than *MB.");

                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlPlacement: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$txtVideoTitle: { required: true },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: true, accept: formats, filesize: "150" },
                        ctl00$ContentPlaceHolder1$txtYouTubeVideoID:{required: true},
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: true },
                        ctl00$ContentPlaceHolder1$txtExpirationDate: { required: true }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlPlacement: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "placement") },
                        ctl00$ContentPlaceHolder1$txtVideoTitle: { required: replaceMessageString(objValMsg, "Required", "video title") },
                        ctl00$ContentPlaceHolder1$fpUpload: { required: replaceMessageString(objValMsg, "Required", "files"), accept: "File type not supported.", filesize: "Please select file less than 150MB." },
                        ctl00$ContentPlaceHolder1$txtYouTubeVideoID: { required: replaceMessageString(objValMsg, "Required", "youtube video id")},
                        ctl00$ContentPlaceHolder1$txtStartDate: { required: replaceMessageString(objValMsg, "Required", "video start date") },
                        ctl00$ContentPlaceHolder1$txtExpirationDate: { required: replaceMessageString(objValMsg, "Required", "news expiration date") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlPlacement")
                            error.insertAfter("#dvPlacement");
                        else
                            error.insertAfter(element);
                    },
                    onsubmit: true
                });

                if ($("#ctl00_ContentPlaceHolder1_hdnEditVideoName").val() != "") {
                    $("#ctl00_ContentPlaceHolder1_fpUpload").rules("remove", "required");
                }

            });


            $("#ctl00_ContentPlaceHolder1_lnkBtnUploadVideo").click(function() {
                $('#dvLoader').show();
                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    $('#dvLoader').hide();
                    return false;
                }
            });

        });

        $(function() {
            $(".datepicker").datepicker({
                buttonText: 'DatePicker',
                showOn: 'button',
                // dateFormat: 'dd-mm-yy',
                buttonImage: '../images/calender-icon.jpg',
                buttonImageOnly: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <div id="dvLoader" style="display:none;">
        <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
        </div>
        <div class="updateProgressDiv">
            <img alt="Loading" src="../Images/ajax-loader-large.gif" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="plhPlayVideo"></asp:PlaceHolder>
    <div class="form_pad">
        <div class="shipping_method_pad" style="width:900px;">
            <h4>
                List Of All Videos - View/Edit and Delete Videos</h4>
            <%--Start list of video with store and workgroup and page name--%>
            <asp:UpdatePanel ID="upPanleMain" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="dtlVideos" />
                </Triggers>
                <ContentTemplate>
                    <div style="text-align: center">
                        <asp:Label ID="lblmsgList" runat="server" CssClass="errormessage"></asp:Label>
                    </div>
                    <div>
                        <asp:GridView ID="dtlVideos" runat="server" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                            GridLines="None" AutoGenerateColumns="false" RowStyle-CssClass="ord_content"
                            OnRowDataBound="dtlVideos_RowDataBound" OnRowCommand="dtlVideos_RowCommand">
                            <Columns>
                                <asp:TemplateField SortExpression="Company">
                                    <HeaderTemplate>
                                        <span>
                                            <asp:LinkButton ID="lnkbtnHeaderCompany" runat="server" CommandArgument="Company"
                                                CommandName="Sort">Company Name</asp:LinkButton>
                                            <asp:PlaceHolder ID="placeholderCompany" runat="server"></asp:PlaceHolder>
                                        </span>
                                        <div class="corner">
                                            <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="first">
                                            <asp:LinkButton ID="lnkbtnItemCompany" CommandName="EditVideo" CommandArgument='<%# Eval("VideoTrainingID") %>'
                                                runat="server" ToolTip="Click here to edit video detail" 
                                                Text='<%# Eval("Company") %>'></asp:LinkButton>
                                        </span>
                                        <div class="corner">
                                            <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="20%"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Workgroup">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderWorkgroup" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span>Workgroup</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderWorkgroup" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblWorkgroup" Text='<%# Eval("Workgroup") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%"/>
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="UserType">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderUserType" runat="server" CommandArgument="UserType" CommandName="Sort"><span>User Type</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderUserType" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblUserType" Text='<%# Eval("UserType") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="15%"/>
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Placement">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderPlacement" runat="server" CommandArgument="Placement" CommandName="Sort"><span>Placement</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderPlacement" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPlacement" Text='<%# Eval("Placement") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="VideoTitle">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderVideoTitle" runat="server" CommandArgument="VideoTitle" CommandName="Sort"><span>Video Title</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderVideoTitle" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblVideoTitle" Text='<%# Eval("VideoTitle") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="15%"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="VideoName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnHeaderVideoName" runat="server" CommandArgument="VideoName"
                                            CommandName="Sort"><span>Video Name</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderVideoName" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <asp:HiddenField ID="hdnVideoName" runat="server" Value='<%# Eval("VideoName") %>' />
                                            <asp:HiddenField ID="hdnYouTubeVideoID" runat="server" Value='<%# Eval("YouTubeVideoID") %>' />
                                            <asp:LinkButton runat="server" ID="lnkbtnItemVideoName" CommandName="DownloadVideo"
                                                CommandArgument='<%# Eval("VideoTrainingID") %>' Text='<% # (Convert.ToString(Eval("VideoName")).Length > 15) ? Eval("VideoName").ToString().Substring(0,15)+"..." : Convert.ToString(Eval("VideoName")) %>'
                                                ToolTip="Click here to download video"></asp:LinkButton>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="20%"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="StartDate">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnStartDate" runat="server" CommandArgument="StartDate"
                                            CommandName="Sort"><span>Start Date</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderStartDate" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStartDate" Text='<%# Eval("StartDate","{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ExpiredDate">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkbtnExpiredDate" runat="server" CommandArgument="ExpiredDate"
                                            CommandName="Sort"><span>Exp. Date</span></asp:LinkButton>
                                        <asp:PlaceHolder ID="placeholderExpiredDate" runat="server"></asp:PlaceHolder>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblExpiredDate" Text='<%# Eval("ExpiredDate","{0:d}") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box"/>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Play</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnPlay" CommandName="PlayVideo"
                                            CommandArgument='<%# Eval("VideoTrainingID") %>' runat="server">
                                            <span class="btn_space">
                                                <img alt="->" id="play" src="~/Images/play.png" runat="server" /></span></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box centeralign"/>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtndelete" CommandName="DeleteVideo" OnClientClick="return confirm('Are you sure, you want to delete selected item?');"
                                            CommandArgument='<%# Eval("VideoTrainingID") %>' runat="server">
                                            <span class="btn_space">
                                                <img alt="X" id="delete" src="~/Images/close.png" runat="server" /></span></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box centeralign"/>
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--End list of video with store and workgroup and page name--%>
            <div class="spacer10">
            </div>
            <div class="btn_space">
                <asp:LinkButton ID="lnkAddVideo" Text="Add Video" CssClass="greysm_btn" runat="server"
                    OnClick="lnkAddVideo_Click"><span>Add Video</span></asp:LinkButton>
            </div>
            <div class="spacer25">
            </div>
            <div class="spacer25">
            </div>
            <div style="text-align: center">
                <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <%--Start table for add/update video based on store,workgroup and department--%>
            <table class="select_box_pad form_table" style="width:360px;">
                <tr>
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 28%">Company Store</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small" style="width:61%;">
                                        <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                                <span class="input_label" style="width: 28%">Workgroup</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small" style="width:61%;">
                                         <asp:DropDownList ID="ddlWorkgroup" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
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
                                <span class="input_label" style="width: 28%">User Type</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small" style="width:61%;">
                                        <asp:DropDownList ID="ddlUserType" runat="server" onchange="pageLoad(this,value);">
                                            <asp:ListItem Text="Both" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Company Admin" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Company Employee" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
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
                                <span class="input_label" style="width: 28%">Video Placement</span>
                                <label class="dropimg_width">
                                    <span class="custom-sel label-sel-small" style="width:61%;">
                                        <asp:DropDownList ID="ddlPlacement" onchange="pageLoad(this,value);" runat="server">
                                        </asp:DropDownList>
                                    </span>
                                    <div id="dvPlacement">
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
                                <span class="input_label" style="width: 28%">Video Title</span>
                                <asp:TextBox ID="txtVideoTitle" CssClass="w_label" runat="server" />
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="checktable_supplier true">
                            <span class="custom-checkbox alignleft" runat="server" id="spnUploadToWL">
                                <asp:CheckBox ID="chkUploadToWL" AutoPostBack="true" runat="server" OnCheckedChanged="chkUploadToWL_CheckedChanged" />
                            </span>
                            <span style="float: left; margin-right: 30px; color: #72757C; line-height: 18px;">Upload to WL</span>
                            <span class="custom-checkbox alignleft" runat="server" id="spnUploadToYouTube">
                                <asp:CheckBox ID="chkUploadToYouTube" AutoPostBack="true" runat="server" OnCheckedChanged="chkUploadToYouTube_CheckedChanged" />
                            </span>
                            <span style="float: left; color: #72757C; line-height: 18px;">Upload to Youtube</span>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trUploadToWL" visible="false">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 28%">UploadVideos</span>
                                <input id="fpUpload" type="file" runat="server" />
                                <br />
                                <div class="noteIncentex" style="width: 100%; font-size: 12px;">
                                    <img src="../Images/lightbulb.gif" alt="note:" />&nbsp;Supported file formats
                                    are <b>.mp4,.mov</b></div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trUploadToYoutube" visible="false">
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%">YouTube Video ID</span>
                                <asp:TextBox ID="txtYouTubeVideoID" CssClass="w_label" runat="server" />
                                <div style="position: absolute; left: 313px; top:0px;">
                                    <a target="_blank" href="http://www.youtube.com/my_videos_upload" title="Login to youtube account">
                                        <img style="width:38px;" src="../Images/YouTubeLogo.png" alt="youtub"/></a>
                                </div>
                                <div style="position: absolute; left: 351px; top:0px;width:100%;color:#72757C;">
                                    User Name : incentexworldlink@gmail.com<br />
                                    Password : incentex
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%">Start Date</span>
                                <asp:TextBox ID="txtStartDate" CssClass="datepicker w_label" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="calender_l">
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 35%">Expiration Date</span>
                                <asp:TextBox ID="txtExpirationDate" CssClass="cal_w datepicker min_w" runat="server"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton runat ="server" ID="lnkBtnUploadVideo" class="grey2_btn" 
                            ToolTip="Upload Video" onclick="lnkBtnUploadVideo_Click"><span>Upload</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <%--End table for add/update video based on store,workgroup and department--%>
        </div>
    </div>
     <asp:UpdatePanel ID="upPrimary" runat="server">
        <ContentTemplate>
            <input id="hdnEditVideoName" runat="server" value="" type="hidden" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
