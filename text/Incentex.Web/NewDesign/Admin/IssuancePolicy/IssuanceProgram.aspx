<%@ Page Title="" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" CodeFile="IssuanceProgram.aspx.cs" Inherits="NewDesign_Admin_IssuancePolicy_IssuanceProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

      $().ready(function() {


          $(".postback").click(function() {
              $(".progress-layer").show();
          });
      });
      function IssuanceDetailsPopup(tabID) {
          $("#employee-Details-Block").css("top", "0").show();
          $(".fade-layer").show();
          popupCenter();
          $.changeTab($("li[tab-id=" + tabID + "]"));
      }
      function IssuanceWizardPopup() {
            $("#dvIPwizard").css("top", "0").show();
            $(".fade-layer").show();
            var srcpath = siteurl + "UserFrameItems/IPWizardSetup.aspx?pID=0";
            $("#iframeWizard").attr("onload", "ShowLoader(false);");
            $("#iframeWizard").attr("src", srcpath);
      }
      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" value="admin-link" id="hdnActiveLink" />
    <%--<% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% }
       else
       { %>--%>
    <div id="container" class="cf filter-page">
        <%--<%} %>--%>
        <div class="narrowcolumn alignleft">
            <asp:LinkButton ID="LnkbtnAddNewIssuance" runat="server" class="blue-btn add-new" OnClick="LnkbtnAddNewIssuance_Click" Text="Add New Issuance Policy"></asp:LinkButton>
            <div class="filter-block cf">
                <div class="title-txt">
                    <span>&nbsp;&nbsp;</span><a href="javascript:;" title="Help video" onclick="ShowVideo();">Help
                        video</a></div>
                <div class="filter-form cf">
                    <ul class="cf">
                        <li>
                            <label class="select-txt filter-text">
                                Select one or more search criteria to open an existing Issuance Policy.
                            </label>
                        </li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchCompany" runat="server" class="default" TabIndex="1"
                                OnSelectedIndexChanged="ddlSearchCompany_SelectedIndexChanged" AutoPostBack="true" >
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchWorkGroup" runat="server" CssClass="default" TabIndex="2">
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchGender" runat="server" CssClass="default" TabIndex="3">
                            </asp:DropDownList>
                        </span></li>
                        <li><span class="select-drop filter-drop">
                            <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="default" TabIndex="4">
                            </asp:DropDownList>
                        </span></li>
                        <li>
                            <asp:Button ID="btnSearchIssuancePolicy" runat="server" OnClick="btnSearchIssuancePolicy_Click"
                                CssClass="blue-btn add-new postback" Text="Search for Issuance Policy" TabIndex="5" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="widecolumn alignright">
            <div class="filter-content">
                <div class="filter-header cf">
                    <span class="title-txt">Issuance Programs</span> <em id="totalcount_em" runat="server"
                        visible="false"></em>
                    <div class="filter-search">
                        <asp:TextBox ID="txtSearchGrid" runat="server" CssClass="input-field-small default_title_text"
                            ToolTip="Search Results..." placeholder="Search Results..." MaxLength="100"></asp:TextBox>
                        <asp:LinkButton ID="btnSearchGrid" runat="server" CssClass="go-btn" OnClick="btnSearchGrid_Click"
                            Enabled="false">
                            GO</asp:LinkButton>
                    </div>
                </div>
                <div class="filter-tableblock cf">
                    <asp:GridView ID="gvIssuancePolicy" runat="server" AutoGenerateColumns="false" GridLines="None"
                        Visible="true" OnRowCommand="gvIssuancePolicy_RowCommand" CssClass="table-grid">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblIssuanceProagramNameHeader" runat="server" CommandArgument="IssuanceProgramName"
                                        CommandName="Sort">Issuance Program Name</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span>
                                        <asp:LinkButton ID="lblIssuanceProgramName" runat="server" CommandName="Detail" CommandArgument='<%# Eval("UniformIssuancePolicyID") %>'
                                            Text='<%# Eval("IssuanceProgramName")%>' CssClass="postback"></asp:LinkButton></span>
                                    <asp:HiddenField ID="hdnUserInfoID" runat="server" Value='<%# Eval("UniformIssuancePolicyID") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="col1 postback" />
                                <ItemStyle CssClass="col1 postback" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lbWorkgroupHeader" runat="server" CommandArgument="Workgroup"
                                        CommandName="Sort">Workgroup</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkgroup" runat="server" Text='<%# Eval("Workgroup") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col2 postback" />
                                <ItemStyle CssClass="col2" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblEligibleDate" runat="server" CommandArgument="EligibleDate"
                                        CommandName="Sort">Station</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEligibleDate" runat="server" Text='<%#Convert.ToDateTime(Eval("EligibleDate")).ToShortDateString()%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col3 postback" />
                                <ItemStyle CssClass="col3" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblIssuanceStatusHeader" runat="server" CommandArgument="IssuanceStatus"
                                        CommandName="Sort">Status</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="apple_check">
                                        <label class="apple_checkbox">
                                            <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Convert.ToBoolean(Eval("IssuanceStatus")) %>'
                                                AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged"></asp:CheckBox>&nbsp;</label></div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="col4" />
                                <ItemStyle CssClass="col4 postback" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="pagingtable" runat="server" class="store-footer cf" visible="false">
                    <a href="javascript:;" class="store-title">BACK TO TOP</a>
                    <asp:LinkButton ID="lnkViewAllBottom" runat="server" OnClick="lnkViewAll_Click" CssClass="pagination alignright view-link postback cf">
                        VIEW ALL
                    </asp:LinkButton>
                    <div class="pagination alignright cf">
                        <span>
                            <asp:LinkButton ID="lnkPrevious" CssClass="left-arrow alignleft postback" runat="server"
                                OnClick="lnkPrevious_Click" ToolTip="Previous">
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
    <div class="popup-outer" id="employee-Details-Block" style="display: none;">
        <div class="popupInner">
            <div class="message-popup employee-pop-block">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);">Help Video</a><a
                    href="javascript:;" class="close-btn" id="aCloseEmployeeDetails">Close</a>
                <div class="message-content employess-content emp-height">
                    <ul class="order-links tabs tabnav cf">
                        <li class="active" tab-id="setup"><a href="javascript:;" title="Setup"><em></em>Setup</a></li>
                        <li tab-id="structures"><a href="javascript:;" title="structures"><em></em>Structures</a></li>
                        <li tab-id="products"><a href="javascript:;" title="Products"><em></em>Products</a></li>
                        <li tab-id="preview"><a href="javascript:;" title="Admin Settings"><em></em>Preview</a></li>
                        <li class="last" tab-id="history"><a href="javascript:;" title="History"><em></em>History</a></li>
                    </ul>
                    <div class="employee-form current-tab setup active">
                        <div>
                            <div class="employee-payment cf active">
                                <ul class="emp-left tabs">
                                    <li class="active" tab-id="workgroup"><a href="javascript:;" title="Workgroup">Workgroup</a></li>
                                    <li tab-id="gender"><a href="javascript:;" title="Gender">Gender</a></li>
                                    <li tab-id="stations"><a href="javascript:;" title="Stations">Stations Options</a></li>
                                    <li tab-id="pricing"><a href="javascript:;" title="Pricing">Pricing</a></li>
                                    <li tab-id="activationperiod"><a href="javascript:;" title="Activation Period">Activation
                                        Period</a></li>
                                    <li tab-id="purchaserequirement"><a href="javascript:;" title="Purchase Requirement">
                                        Purchase Requirement</a></li>
                                    <li tab-id="marketingcommunications"><a href="javascript:;" title="Marketing Communications">
                                        Marketing Communications</a></li>
                                    <li tab-id="issuancecategory"><a href="javascript:;" title="Issuance Category">Issuance
                                        Category</a></li>
                                    <li tab-id="nameofpolicy"><a href="javascript:;" title="NameofPolicy">Name of Policy</a></li>
                                </ul>
                                <div class="emp-right current-tab paymentoptions">
                                    <div class="emp-title">
                                        Workgroup</div>
                                </div>
                                <div class="asset-btn-block assetbtn-blockbar cf">
                                    <a id="aBasicCancel" class="small-gray-btn" href="javascript:;" title="Cancel changes and Close">
                                        <span>CANCEL</span> </a>
                                    <asp:LinkButton ID="lbBasicSave" CssClass="small-blue-btn submit" ToolTip="Save and Move to Next Tab"
                                        ValidationGroup="Basic" call="Basic" runat="server">
                                    <span>SAVE</span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lbBasicClose" runat="server" CssClass="small-gray-btn" ValidationGroup="Basic"
                                        call="Basic" ToolTip="Save changes and Close">
                                    <span>CLOSE</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="employee-form current-tab structures" style="display: none;">
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
    </div>
    
     <div id="dvIPwizard" class="popup-outer" style="display: none;">
         <div class="popupInner">
            <div class="message-popup employee-pop-block">
                <a class="help-video-btn" title="Help Video" href="javascript: void(0);">Help Video</a><a
                    href="javascript:;" class="close-btn" id="a1">Close</a>
                    <iframe id="iframeWizard"></iframe>
                </div>
            </div>
    </div>
</asp:Content>
