<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="Assets.aspx.cs" Inherits="Admin_Assets" Title="incentex | Assets" EnableEventValidation="false"
    EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function(){
            $(window).ValidationUI();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetTriggeredElement);
        });
        function GetTriggeredElement()
        {
            $(".default").uniform();
            ScrollToTag("container",false);
            
            $('.bulk-upload').click(function () {
                ShowPopUp("dvBasicBulkPopup", "dvBasicBulkPopup .warranty-content");
                Page_ClientValidateReset($(".error"));
            });

            BindPopUpCloseEvents();
        }
        function ShowPopUp(MainDivTargetID, PopUpDivTargetID) {
            $("#" + MainDivTargetID).css('top', '0');
            $("#" + PopUpDivTargetID).show();
            $(".fade-layer").show();
            SetPopUpAtTop();
        }
    </script>

    <%--<script src="../../NewDesign/js/multiupload.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            var config = {
	            support : "image/jpg,image/png,image/bmp,image/jpeg,image/gif",		// Valid file formats
	            divID: "dvBasicBulkPopup",					// div ID
	            dragArea: "dragAndDropFiles",		// Upload Area ID
	            uploadUrl: "upload.php",				// Server side upload url
	            inputButtonClass: "fileupload",
	            SubmitButtonClass: "btnuploadFile"
            };
            
               
           
	        initMultiUploader(config);
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc" runat="server">
    </asp:ScriptManager>
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <asp:UpdatePanel ID="upAssests" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearchAsset" />
        </Triggers>
        <ContentTemplate>
            <section id="container" class="cf GSE-page">
                <div class="narrowcolumn alignleft">
                    <a href='<%= System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "NewDesign/Admin/AddNewAsset.aspx" %>'
                        class="blue-btn add-new">Add Asset Manually</a> <a href="javascript: void(0);" class="blue-btn add-new bulk-upload">
                            Bulk Upload Assets</a>
                    <div class="filter-block cf">
                        <div class="title-txt">
                            <span>Search</span><a href="javascript:void(0);" onclick="GetHelpVideo('Asset Management','Gse Management');" title="Help video">Help video</a></div>
                        <div class="filter-form cf">
                        <ul class="cf">
                            <li>
                                <asp:TextBox ID="txtCompanyStore" runat="server" CssClass="input-field-all default_title_text" placeholder="Company Store..."
                                    ToolTip="Company Store..." tabindex="1" Visible="false"></asp:TextBox></li>
                          <li><span class="select-drop filter-drop">
                                <asp:DropDownList ID="ddlBaseStation" runat="server" class="default" OnSelectedIndexChanged="ddlBaseStation_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList></span></li>
                            <li><span class="select-drop filter-drop">
                                <asp:DropDownList ID="ddlAssetType" runat="server" class="default" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList></span></li>
                            <li><span class="select-drop filter-drop">
                              <asp:DropDownList ID="ddlAssetID" runat="server" class="default" OnSelectedIndexChanged="ddlAssetID_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList></span></li>
                              <li><span class="select-drop filter-drop">
                                 <asp:DropDownList ID="ddlDepartments" runat="server" class="default">
                                                    </asp:DropDownList></span></li>
                            <li><span class="select-drop filter-drop">
                               <asp:DropDownList ID="ddlStatus" runat="server" class="default" >
                                                    </asp:DropDownList></span></li>
                            <li>
                                <button id="btnSearchAsset" runat="server" class="blue-btn" onserverclick="btnSearchAsset_Click">
                                    Search</button></li>
                        </ul>
                        </div>
                    </div>
                </div>
                <div class="widecolumn alignright">
                    <div class="filter-content" id="GseManagement">
                        <div class="filter-headbar cf">
                            <span class="headbar-title">GSE Management</span> <em id="totalcount_em" runat="server" visible="false"></em>
                            <div class="filter-search">
                                <input type="text" class="input-field-small default_title_text" title="Search Results..."
                                    placeholder="Search Results..." id="txtSearchGo" runat="server" />
                                <button id="btnGo" runat="server" class="go-btn" onserverclick="btnGo_Click">
                                    GO</button></div>
                        </div>
                        <div class="gse-tablebox cf">
						
                            <asp:GridView ID="gvAssests" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvAssets_RowCommand" OnRowDataBound="gvAssets_RowDataBound" CssClass="table-grid">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnAssetID" runat="server" CommandArgument="EquipmentID" CommandName="Sort"><span style="text-align:center" >Asset ID</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="lnkedit" CommandArgument='<%# Eval("EquipmentMasterID") %>' CommandName="detailCamp"
                                                runat="server" NavigateUrl='<%# "~/AssetManagement/AssetProfile.aspx?Id=" + Eval("EquipmentMasterID").ToString()%>'><span><%# Eval("EquipmentID")%></span></asp:HyperLink>--%>
                                            <asp:LinkButton ID="lnkbtnEquipmentID" CssClass="link" runat="server" PostBackUrl='<%# System.Configuration.ConfigurationManager.AppSettings["siteurl"] + "NewDesign/Admin/AddNewAsset.aspx?eqpID=" + Eval("EquipmentMasterID") %>' Text='<%# Eval("EquipmentID") %>'> </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col1" />
                                        <ItemStyle CssClass="col1" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="EquipmentType">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnEquipmentType" runat="server" CommandArgument="EquipmentType"
                                                CommandName="Sort"><span >Asset Type</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfEquipmentType" runat="server" Value='<%# Eval("EquipmentType")%>' />
                                            <asp:Label runat="server" ID="lblEquipmentType" Text='<%# Eval("EquipmentType")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col2" />
                                        <ItemStyle CssClass="col2" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="BaseStation">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnBaseStation" runat="server" CommandArgument="BaseStation"
                                                CommandName="Sort"><span>Station</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfBaseStation" runat="server" Value='<%# Eval("BaseStation")%>' />
                                            <asp:Label runat="server" ID="lblBaseStation" Text='<%# Convert.ToString(Eval("BaseStation")).Length > 3 ? Convert.ToString(Eval("BaseStation")).Substring(0,3) : Eval("BaseStation") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col3 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <ItemStyle CssClass="col3 text-center" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Status">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnStatus" runat="server" CommandArgument="Status" CommandName="Sort"><span >Status</span></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("Status")%>' />
                                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col4 text-center" />
                                        <ItemStyle CssClass="col4 text-center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table-grid">
        	                            <tr>
          	                            <th class="col1">Asset ID</th>
                                        <th class="col2">Asset Type</th>
                                        <th class="col3">Station</th>
                                        <th class="col4">Status</th>
                                      </tr>
                                      <tr>
          	                            <td colspan="4" style="text-align:center;vertical-align:middle;">Records not found</td>
                                      </tr>
                                       
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div class="store-footer cf" id="pagingtable" runat="server" visible="false">
                           <a href="javascript: void(0);" class="store-title">BACK TO TOP</a>
                           <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" CssClass="pagination alignright view-link postback cf">
                                VIEW ALL
                            </asp:LinkButton>
                             <div class="pagination alignright cf" >
                                <asp:LinkButton ID="lnkbtnPrevious" class="left-arrow alignleft" runat="server" OnClick="lnkbtnPrevious_Click"> 
                                </asp:LinkButton>
                                <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                </asp:DataList>
                                <asp:LinkButton ID="lnkbtnNext" class="right-arrow alignright" runat="server" OnClick="lnkbtnNext_Click"> 
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="dvBasicBulkPopup" class="popup-outer">
        <div class="popupInner">
            <div class="basic-bulk-popup">
                <a href="javascript:void(0);" onclick="GetHelpVideo('Asset Management','Bulk Upload Assets')" title="Help Video" class="help-video-btn">Help Video</a> <a class="close-btn"
                    href="javascript:void(0);">Close</a>
                <div class="filter-content">
                    <div class="filter-headbar cf">
                        <span class="headbar-title">Bulk Upload Assets</span>
                    </div>
                    <div class="messagediv cf" style="display: none;">
                        <asp:Label ID="lblMsg" runat="server" Text="Assets added successfully"></asp:Label>
                    </div>
                    <div class="upload-form">
                        <div id="dragAndDropFiles" class="uploadArea upload-content">
                            <div class="upload-txt">
                                Drag or paste your Excel file here, or <a title="browse" href="#">browse</a> for
                                a file to upload.</div>
                            <div class="upload-file">
                                <asp:FileUpload ID="fuBulkAssetFile" runat="server" CssClass="fileselect checkvalidation" />
                                <%--<input type="file" name="multiUpload" id="multiUpload" runat="server" class="fileupload checkvalidation" />--%>
                                <asp:RequiredFieldValidator ID="rqBulkAssetFile" runat="server" ControlToValidate="fuBulkAssetFile"
                                    Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="SaveBulkAsset"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%--<div style="width:400px;border: black 3px dashed;">
	                        <div id="filedrag">
		                        <label for="fileselect">Files to upload:</label>
	                            <input type="file" id="fileselect" name="fileselect[]" multiple="multiple" class="fileselect" />
	                            or drop files here</div>
                        </div>--%>
                        <asp:LinkButton ID="lnkbtnDownload" runat="server" CssClass="blue-btn" Text="Download Template"
                            OnClick="lnkbtnDownload_Click" ></asp:LinkButton>
                        <asp:Button ID="lnkbtnUploadBulkAsset" runat="server" CssClass="blue-btn submit"
                            Text="Upload" OnClick="lnkbtnUploadBulkAsset_Click" ValidationGroup="SaveBulkAsset"
                            call="SaveBulkAsset"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
