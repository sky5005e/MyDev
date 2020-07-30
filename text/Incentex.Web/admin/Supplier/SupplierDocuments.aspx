<%@ Page  Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="SupplierDocuments.aspx.cs" Inherits="admin_Supplier_SupplierDocuments" Title="Supplier Documents" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl" TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        $().ready(function() {
            /*Function to get X and Y co-ordinates of a browser*/

            posY = getScreenCenterY();
            posX = getScreenCenterX();

            $("#ctl00_ContentPlaceHolder1_hfY").val(posY);
            $("#ctl00_ContentPlaceHolder1_hfX").val(posX);

            function getScreenCenterY() {
                var y = 0;
                y = getScrollOffset() + (getInnerHeight() / 2);
                return (y);
            }

            function getScreenCenterX() {
                return (document.body.clientWidth / 2);
            }

            function getInnerHeight() {
                var y;
                if (self.innerHeight) // all except Explorer
                {
                    y = self.innerHeight;
                }
                else if (document.documentElement &&
            document.documentElement.clientHeight)
                // Explorer 6 Strict Mode
                {
                    y = document.documentElement.clientHeight;
                }
                else if (document.body) // other Explorers
                {
                    y = document.body.clientHeight;
                }
                return (y);
            }

            function getScrollOffset() {
                var y;
                if (self.pageYOffset) // all except Explorer
                {
                    y = self.pageYOffset;
                }
                else if (document.documentElement &&
            document.documentElement.scrollTop)
                // Explorer 6 Strict
                {
                    y = document.documentElement.scrollTop;
                }
                else if (document.body) // all other Explorers
                {
                    y = document.body.scrollTop;
                }
                return (y);
            }

            /*End*/

            $("#ctl00_ContentPlaceHolder1_btnUpload").click(function() {
            //alert("click");
                return $('#aspnetForm').valid();
            }); //click

                $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {

                    objValMsg = $.xml2json(xml);

                    $("#aspnetForm").validate({
                        rules: {
                            ctl00$ContentPlaceHolder1$flu: { required: true }
                        }
                        ,
                        messages:
                        {
                            ctl00$ContentPlaceHolder1$flu: {
                                required: replaceMessageString(objValMsg, "Required", "file name")
                            }
                        },
                        onsubmit: false

                    }); //validate
                });
            });
      
    
    </script>
    
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="sc1" runat="server">
    </ajax:ToolkitScriptManager>
                <mb:MenuUserControl ID="menuControl" runat="server" />

    <div class="form_pad">
        <div style="text-align: center">
            <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
        </div>
        <h4>
            Supplier Setup Documents</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <%--<a href="#" class="greyicon_btn btn" title="Social Security Card"><span class="btn_width275">Social Security Card<img src="images/social-security-icon.png" alt=""/></span></a>--%>
                        <asp:LinkButton ID="lnkSocialSecurityCard" runat="server" class="greyicon_btn btn"
                            OnClick="lnkSocialSecurityCard_Click"> 
								        <span class="btn_width275">Social Security Card<img src="../../images/social-security-icon.png" alt=""/></span>    
                        </asp:LinkButton>
                    </td>
                    <td class="formtd">
                        <%--<a href="#" class="greyicon_btn btn" title="W-4 & I-9 Documents"><span class="btn_width275">W-4 & I-9 Documents<img src="images/upload-other-icon.png" alt=""/></span></a>--%>
                        <asp:LinkButton ID="lnkW4I9Documents" runat="server" CssClass="greyicon_btn btn"
                            OnClick="lnkW4I9Documents_Click">
								        <span class="btn_width275">W-4 & I-9 Documents<img src="../../images/upload-other-icon.png" alt=""/></span>    
                        </asp:LinkButton>
                    </td>
                    <td class="formtd_r">
                        <%--<a href="#" class="greyicon_btn btn" title="Valid Photo ID"><span class="btn_width275">Valid Photo ID<img src="images/valid-photo-icon.png" alt=""/></span></a>--%>
                        <asp:LinkButton ID="lnkValidPhotoID" runat="server" CssClass="greyicon_btn btn" OnClick="lnkValidPhotoID_Click">
								        <span class="btn_width275">Valid Photo ID<img src="../../images/valid-photo-icon.png" alt=""/></span>    
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="formtd" colspan="3">
                        <%--<a href="#" class="greyicon_btn btn" title="Signed NDA Agreement"><span class="btn_width275">Signed NDA Agreement<img src="images/signed-icon.png" alt=""/></span></a>--%>
                        <asp:LinkButton ID="lnkSignedNDAAgreement" runat="server" CssClass="greyicon_btn btn"
                            OnClick="lnkSignedNDAAgreement_Click">
								        <span class="btn_width275">Signed NDA Agreement<img src="../../images/signed-icon.png" alt=""/></span>    
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding-bottom: 2px">
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                        
                    
                        <!-- document grid -->
                        
                        <asp:GridView ID="gvSetupDocuments" runat="server" RowStyle-CssClass="ord_content"
                            AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box"
                            GridLines="None"  Width="70%" OnDataBinding="gvSetupDocuments_DataBinding"
                            OnRowCommand="gvSetupDocuments_RowCommand" 
                            OnRowDataBound="gvSetupDocuments_RowDataBound" 
                            ondatabound="gvSetupDocuments_DataBound">
                            <RowStyle CssClass="ord_content"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Id">
                                    <HeaderTemplate>
                                        <span>Document ID</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%-- <span >File Type</span>--%>
                                        <asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileType" CommandName="Sort"><span >File Type</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileType" runat="server" Text='<%#Eval("FileType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%--<span >File Name</span>--%>
                                        <asp:LinkButton ID="lnkbtnFileNameHead" runat="server" CommandArgument="FileName"
                                            CommandName="Sort"><span >File Name</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <%-- <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                    Text='<%# Eval("FileName") %>'></asp:LinkButton>--%>
                                            <asp:HyperLink ID="lnkDoc" runat="server" Text='<%#Eval("OriginalFileName")%>' NavigateUrl='<%# "~/UploadedImages/IncentexEmployeeDocuments/" + Eval("FileName")%>'
                                                onclick="window.open(this.href);return false;"> </asp:HyperLink>
                                            <%--  <a href="about:blank" onclick="window.open(this.href);return false;" >Open</a>--%>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="default_btninput">
                                            <asp:HiddenField ID="hdnFileName" runat="server" Value='<%#Eval("FileName")%>' />
                                            &nbsp;&nbsp;&nbsp;
                                           <%-- <asp:Button ID="lnkbtndelete" CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                CommandArgument='<%#Eval("DocumentId")%>' Text="X" runat="server" />--%>
                                                  <asp:ImageButton ID="lnkbtndelete"
                                                CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                runat="server"
                                                CommandArgument='<%#Eval("DocumentId")%>' 
                                                ImageUrl="~/Images/close.png" 
                                                style="background:transparent"
                                                />
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        <%--style="float: left"--%>
                        
                        <div style="width:70%">
                        <div class="alignright pagging" >
                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"></asp:LinkButton>
                            
                            <span>
                                        <asp:DataList ID="lstPaging" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                            OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow"
                                            >
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        </span>
                                    
                                        <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                                    
                        </div>
                        </div>
                        <!-- document grid  end-->
                    </td>
                </tr>
            </table>
        </div>
        <div class="divider" style="margin-top: 1px">
        </div>
        <h4>
            Additional Supplier Documents</h4>
        <div>
            <table class="form_table">
                <tr>
                    <td class="formtd">
                        <%--<a href="#" class="greyicon_btn btn" title="Annual Review"><span class="btn_width275">Annual Review<img src="images/annual-review-icon.png" alt=""/></span></a>--%>
                        <asp:LinkButton ID="lnkAnnualReview" runat="server" CssClass="greyicon_btn btn" OnClick="lnkAnnualReview_Click">
								        <span class="btn_width275">Annual Review<img src="../../images/annual-review-icon.png" alt=""/></span>
                        </asp:LinkButton>
                    </td>
                    <td class="formtd">
                        <asp:LinkButton ID="lnkOtherDocuments" runat="server" CssClass="greyicon_btn btn"
                            OnClick="lnkOtherDocuments_Click">
								    <span class="btn_width275">Other Documents<img src="../../images/upload-other-icon.png" alt=""/></span>
                        </asp:LinkButton>
                        <%--<a href="#" class="greyicon_btn btn" title="Other Documents"><span class="btn_width275">Other Documents<img src="images/upload-other-icon.png" alt=""/></span></a>--%>
                    </td>
                    <td class="formtd_r">
                        &nbsp;
                    </td>
                </tr>
              
                <tr>
                    <td colspan="3" style="padding-bottom: 2px">
                        <!-- document grid -->
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                        
                    
                        <asp:GridView ID="gvAdditional" runat="server" RowStyle-CssClass="ord_content" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="ord_header" CssClass="orderreturn_box" GridLines="None"
                             Width="70%" 
                            ondatabinding="gvAdditional_DataBinding" 
                            onrowcommand="gvAdditional_RowCommand" ondatabound="gvAdditional_DataBound">
                            <RowStyle CssClass="ord_content"></RowStyle>
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Id">
                                    <HeaderTemplate>
                                        <span>Document ID</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblDocumentID" Text='<%# Eval("DocumentId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%-- <span >File Type</span>--%>
                                        <asp:LinkButton ID="lnkbtnFileName" runat="server" CommandArgument="FileType" CommandName="Sort"><span >File Type</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileType" runat="server" Text='<%#Eval("FileType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" Width="30%"/>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%--<span >File Name</span>--%>
                                        <asp:LinkButton ID="lnkbtnFileNameHead" runat="server" CommandArgument="FileName"
                                            CommandName="Sort"><span >File Name</span></asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span>
                                            <%-- <asp:LinkButton runat="server" ID="lnkFileName" CommandName="view" CommandArgument='<%# Eval("FileName") %>'
                                                                    Text='<%# Eval("FileName") %>'></asp:LinkButton>--%>
                                            <asp:HyperLink ID="lnkDoc" runat="server" Text='<%#Eval("OriginalFileName")%>' NavigateUrl='<%# "~/UploadedImages/IncentexEmployeeDocuments/" + Eval("FileName")%>'
                                                onclick="window.open(this.href);return false;"> </asp:HyperLink>
                                            <%--  <a href="about:blank" onclick="window.open(this.href);return false;" >Open</a>--%>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="b_box" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <span>Delete</span>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="default_btninput">
                                            <asp:HiddenField ID="hdnFileName" runat="server" Value='<%#Eval("FileName")%>' />
                                            &nbsp;&nbsp;&nbsp;
                                           <%-- <asp:Button ID="lnkbtndelete" 
                                            CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                CommandArgument='<%#Eval("DocumentId")%>' Text="X" runat="server" />--%>
                                                <asp:ImageButton ID="lnkbtndelete"
                                                CommandName="del" OnClientClick="return confirm('Are you sure, you want to delete selected records ?');"
                                                runat="server"
                                                CommandArgument='<%#Eval("DocumentId")%>' 
                                                ImageUrl="~/Images/close.png" 
                                                style="background:transparent"
                                                />
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="g_box" Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                         <div style="width:70%">
                        <div class="alignright pagging" >
                         <asp:LinkButton ID="lnkbtnPreviousAdditional" class="prevb" runat="server" 
                                            onclick="lnkbtnPreviousAdditional_Click"></asp:LinkButton>
                                            <span>
                                        <asp:DataList ID="lstAdditional" runat="server" CellPadding="1" CellSpacing="1" OnItemCommand="lstPaging_ItemCommand"
                                            OnItemDataBound="lstPaging_ItemDataBound" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow"
                                            >
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        </span>
                                        <asp:LinkButton ID="lnkbtnNextAdditional" class="nextb" runat="server" 
                                            onclick="lnkbtnNextAdditional_Click"></asp:LinkButton>    
                                            
                           
                        </div>
                        </div>
                        <!-- document grid  end-->
                    </td>
                </tr>
            </table>
        </div>
        <div class="alignnone spacer25">
        </div>
        <div class="additional_btn">
            <ul class="clearfix">
                <li>
                    <%--<a href="#" title="Save Information" class="grey2_btn"><span>Save Information</span></a>--%>
                    <%--<asp:LinkButton ID="lnkNext" runat="server" CssClass="grey2_btn" OnClick="lnkNext_Click">
								        <span>Netxt >></span>
                    </asp:LinkButton>--%>
                </li>
            </ul>
        </div>
    </div>
    <!-- Upload Document Pop up -->
    <input type="hidden" id="hfX" value="" runat="server" />
    <input type="hidden" id="hfY" value="" runat="server" />
    <asp:LinkButton ID="lnkDummyAddNewMasterPrice" class="grey2_btn alignright" runat="server"
        Style="display: none"></asp:LinkButton>
    <ajax:ModalPopupExtender ID="modal" TargetControlID="lnkDummyAddNewMasterPrice" BackgroundCssClass="modalBackground"
        DropShadow="true" runat="server" PopupControlID="pnlDocumentMasterPrice" CancelControlID="closepopupMasterPrice">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlDocumentMasterPrice" runat="server">
        <div class="pp_pic_holder facebook" style="display: block; width: 411px;">
            <div class="pp_top" style="">
                <div class="pp_left">
                </div>
                <div class="pp_middle">
                </div>
                <div class="pp_right">
                </div>
            </div>
            <div class="pp_content_container" style="" id="divPopUp" name="divPopUp">
                <div class="pp_left" style="">
                    <div class="pp_right" style="">
                        <div class="pp_content" style="height: 128px; display: block;">
                            <div class="pp_loaderIcon" style="display: none;">
                            </div>
                            <div class="pp_fade" style="display: block;">
                                <div id="Div2">
                                    <div class="pp_inline clearfix">
                                        <div class="form_popup_box">
                                            <div class="label_bar">
                                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Size="14px"></asp:Label>
                                            </div>
                                            <div class="label_bar">
                                                <label>
                                                    Upload File :</label>
                                                <span>
                                                    <asp:FileUpload ID="flu" runat="server" />
                                                </span>
                                            </div>
                                            <div class="label_bar btn_padinn">
                                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="btnUpload_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pp_details clearfix" style="width: 371px;">
                                    <a href="#" id="closepopupMasterPrice" runat="server" class="pp_close">Close</a>
                                    <p class="pp_description" style="display: none;">
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pp_bottom" style="">
                <div class="pp_left" style="">
                </div>
                <div class="pp_middle" style="">
                </div>
                <div class="pp_right" style="">
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- Upload Document Pop up end -->
    
    
</asp:Content>

