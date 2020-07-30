<%@ Page Title="incentex | Pending Orders" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master"
    AutoEventWireup="true" CodeFile="PendingOrders.aspx.cs" Inherits="MyAccount_OrderManagement_PendingOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function ShowReasonForCancelPopup(OrderID) {
            $("#CancelOrder-popup").css('top', '0');
            $(".fade-layer").show();
            $("#ctl00_ContentPlaceHolder1_pnlCancelOrder").show();
            // set id to hidden field
            $("#ctl00_ContentPlaceHolder1_hdnCancelOrderID").val(OrderID); 
            $("#ctl00_ContentPlaceHolder1_txtReasonCode").val('');
        }
        function DisplayShippingInfo() {

            // $("#ShippingAddress-popup").css('top', '0');
            $("#register-block").css('top', '0');
            $(".fade-layer").show();
            $("#ctl00_ContentPlaceHolder1_pnlEditShippingAddress").show();
        }
        function CloseShippingInfo() {
            $("#register-block").css('top', '-9999px');
            $(".fade-layer").hide();
            $("#ctl00_ContentPlaceHolder1_pnlEditShippingAddress").hide();
        }
        // Display Child Grid view
        function showOrderDetailsGV(obj) {
            var ChildGridView = document.getElementById(obj);
            if (ChildGridView.style.display == "none") {
                ChildGridView.style.display = "inline";

            } else {
                ChildGridView.style.display = "none";
            }
        }
        //        // Select All checkbox
        //        function changeAll(chk) {
        //            var parent = getParentByTagName(chk, "table");
        //            var checkBoxes = parent.getElementsByTagName("input");
        //            for (var i = 0; i < checkBoxes.length; i++)
        //                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].id.indexOf("chkSelectOrder") >= 0)
        //                checkBoxes[i].checked = chk.checked;
        //        }
        //        function getParentByTagName(obj, tag) {
        //            var obj_parent = obj.parentNode;
        //            if (!obj_parent) return false;
        //            if (obj_parent.tagName.toLowerCase() == tag) return obj_parent;
        //            else return getParentByTagName(obj_parent, tag);
        //        }
    </script>

    <script type="text/javascript">

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                GeneralAlertMsg("Please enter numeric value");
                txt.value = '';
                txt.focus();
            }

        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
            //var ValidChars = "0123456789";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }
        $().ready(function() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat'
                //radioClass: 'iradio_flat'
            });
            $(window).ValidationUI();
            $('.table-headbtn a').click(function() {
                var _title = $(this).attr("title");
                var strfor = _title;
                if (_title == "Approve Selected") {
                    _title = 'To approve the order(s) you selected, please click "yes" to confirm.';
                }
                else {
                    _title = 'Are you sure you want to ' + _title + '?';
                }
                DisplayConfirmation(_title, strfor);
            });
        });

        function DisplayConfirmation(_msg,strfor) {
            $("#Confirmation-popup").css('top', '0');
            $(".fade-layer").show();
            $("#dvConfirmationMsg").show();
            $("#pdMsg").html(_msg);
            $("#ctl00_ContentPlaceHolder1_hdnMainStatus").val(strfor);
        }

        function CloseConfirmation() {
            $("#Confirmation-popup").css('top', '-9999px');
            $(".fade-layer").hide();
            $("#dvConfirmationMsg").hide();
            $("#pdMsg").html('');
            $("#ctl00_ContentPlaceHolder1_hdnMainStatus").val('');
        }
        //End
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section id="container" class="cf pendingOrder-page">
	<div class="pending-order-block">
  	<h5 class="filter-headbar cf"><span class="headbar-title">Pending Orders</span> <span class="watch-video-org">
  	<a href="javascript: void(0);" class="video-btn popup-openlink" title="Watch Help video" onclick="GetHelpVideo('Pending Orders','Pending Orders')"></a></span></h5>
		<div class="table-headbtn cf">
			<a  href="javascript:;" class="table-btn alignleft" title="Approve Selected" >Approve Selected</a>
			<a  href="javascript:;" class="table-btn alignleft" title="Approve All" >Approve All</a>
			<a href="javascript:;" class="table-btn del-button alignleft" title="Cancel All" >Cancel All</a>
			<asp:Label ID="lblPendingCount" runat="server" class="pending-ordr"></asp:Label>
		</div>
  	
       <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvPendingOrders_RowDataBound" OnRowCommand="gvPendingOrders_RowCommand" CssClass="table-grid">
                    <Columns>
                       <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderID" Text='<%# Eval("OrderID") %>' />
                                <asp:Label runat="server" ID="lblOrderStatus" Text='<%# Eval("OrderStatus") %>' />
                                <asp:Label runat="server" ID="lblOrderFor" Text='<%# Eval("OrderFor") %>' />
                                 <asp:HiddenField runat="server" ID="hdnUser" Value='<%# Eval("UserName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>&nbsp;
                            <%--<label class="label_check"><asp:CheckBox ID="cbSelectAll" runat="server" OnClick="changeAll(this)"/>&nbsp;</label>--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <label class="label_checkbox"><asp:CheckBox CssClass="icheckbox_flat" ID="chkSelectOrder" runat="server" />&nbsp; </label>                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField SortExpression="OrderDate">
                            <HeaderTemplate >
                                 <asp:LinkButton ID="lnkRequestDateHeader" runat="server" CommandArgument="OrderDate"
                                    CommandName="Sorting">Request Date</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderSubmitedDate" Text='<%# Convert.ToDateTime(Eval("OrderDate")).ToShortDateString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField SortExpression="FullName">
                            <HeaderTemplate>
                               <asp:LinkButton ID="lnkRequestorHeader" runat="server" CommandArgument="FullName"
                                    CommandName="Sorting">Requestor</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblUserName" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="ReferenceName">
                            <HeaderTemplate>
                             <asp:LinkButton ID="lnkReferenceName" runat="server" CommandArgument="ReferenceName"
                                    CommandName="Sorting">Reference Name</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblReferenceName" Text='<%# Eval("ReferenceName").ToString().Length>20 ? (Eval("ReferenceName") as string).Substring(0,20)+".." : Eval("ReferenceName")%>' ToolTip='<%# Eval("ReferenceName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField SortExpression="sBaseStation">
                            <HeaderTemplate>
                               <asp:LinkButton ID="lnkStationHeader" runat="server" CommandArgument="sBaseStation"
                                    CommandName="Sorting">Station</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblBaseStationName" Text='<%# Convert.ToString(Eval("BaseStation")).Substring(0, 3)%>' />
                            </ItemTemplate>
                             <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="TotalAmount">
                            <HeaderTemplate>
                            <asp:LinkButton ID="lnkTotalAmount" runat="server" CommandArgument="TotalAmount"
                                    CommandName="Sorting">Amount</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOrderPrice" Text='<%# Convert.ToDecimal(Eval("TotalAmount")).ToString("C2") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="textright" />
                            <ItemStyle CssClass="textright" />
                        </asp:TemplateField>
                      
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Actions</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <div class="pending-btn">
                            <%--<asp:LinkButton ID="lnkApprove" runat="server" class="gray-button" title="APPROVE"  CommandArgument='<%# Eval("OrderID") %>' CommandName="Approve">APPROVE</asp:LinkButton>--%>
                            <a href="javascript:showOrderDetailsGV('Order-<%# Eval("OrderID") %>');" class="gray-button royaledit-btn" title="EDIT">EDIT</a>
                            <a href="javascript: void(0);" class="gray-button del-button" title="CANCEL" onclick="ShowReasonForCancelPopup(this.id);" id='<%# Eval("OrderID") %>'>CANCEL</a>
                            </div>
                            </ItemTemplate>
                            <HeaderStyle CssClass="last last-action textcenter" />
                            <ItemStyle CssClass="last last-action textcenter" />
                        </asp:TemplateField>
                                               
                        <asp:TemplateField>
                        <ItemStyle Width="0px" />
                        <HeaderStyle Width="0px" />
                        <ItemTemplate>
                        <tr class="no-border">
                            <td colspan="100%" class="no-padding">
                                <div id="Order-<%# Eval("OrderID") %>" class="subOrderItem" style="display: none; position: relative;">
                                <div class="table-griddesc">
      	                        <span class="top-arrow"></span>
                                    <asp:GridView ID="gvOrderDetails" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table-grid" OnRowCommand="gvOrderDetails_RowCommand">
                                        <Columns>
                             <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMyCartID" runat="server" Text='<%# Eval("MyCartID") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>                                                          
                            <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Item #</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemNum"  runat="server" Text='<%# Eval("ItemNumber") %>'/>
                            </ItemTemplate>
                         
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Product Description</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProductDescription" runat="server" Text='<%# Eval("ProductDescription") %>'/>
                            </ItemTemplate>
                            <HeaderStyle CssClass="proddesc-td" />
                            <ItemStyle CssClass="proddesc-td" />                            
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Size</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSize" runat="server" Text='<%# Eval("Size") %>'/>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Ordered</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'/>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                          </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Change Quantity</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="txtqty" runat="server"  class="input-field-small input-qty" onchange="CheckNum(this.id)"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Unit Cost</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                
                                  <asp:Label ID="lblUnitCost" Text=' <%# Convert.ToDecimal(Eval("UnitPrice")).ToString("C2")%>'
                                                            runat="server"></asp:Label>
                                                         <%--   <asp:Label ID="lblMOASUnitCost" Text=' <%# Convert.ToDecimal(Eval("MOASPrice")).ToString("C2")%>'
                                                            runat="server"></asp:Label>--%>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textright" />
                            <ItemStyle CssClass="textright" />
                        </asp:TemplateField>
                      
                          <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Subtotal</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblExtendedPrice" Text='<%# Convert.ToDecimal((Convert.ToDecimal(Eval("UnitPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("C2")%>'
                                                            runat="server"></asp:Label>
                                                          <%--  <asp:Label ID="lblMOASExtendedPrice" Text='<%# Convert.ToDecimal((Convert.ToDecimal(Eval("MOASPrice")) * Convert.ToDecimal(Eval("Quantity")))).ToString("C2")%>'
                                                            runat="server"></asp:Label>--%>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textright" />
                            <ItemStyle CssClass="textright" />
                        </asp:TemplateField>
                          <asp:TemplateField>
                            <HeaderTemplate>
                                <span>Action</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnlDeleteItem" runat="server" CommandArgument='<%# Eval("OrderID")+","+Eval("MyCartID") %>' CommandName="DeleteOrderItem"  class="gray-button del-button" title="DELETE">DELETE</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle CssClass="textcenter" />
                            <ItemStyle CssClass="textcenter" />
                        </asp:TemplateField>
                       
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <table class="table-grid subtotal-grid">
  		                            <tr>
  		                            <td style="width:148px;">&nbsp;</td>
      	                            <td class="col1">Subtotal</td>
                                    <td class="col2"><asp:Label ID="lblSubTotalOrderAmount" Text='<%# Convert.ToDecimal(Eval("OrderAmount")).ToString("C2")%>'
                                                                                    runat="server"></asp:Label></td>
                                    <td class="spacetd"></td>
                                  </tr>
                                 <%-- <tr>
                                  <td></td>
      	                            <td class="col1">Promo Code Discount</td>
                                    <td class="col2"><asp:Label ID="lblPromoCodeDiscountAmount" Text='<%# "(" +Convert.ToDecimal(Eval("CorporateDiscount")).ToString("C2") + ")"%>'
                                                                                    runat="server"></asp:Label></td>
                                  </tr>--%>
                                  <tr>
                                  <td></td>
      	                            <td class="col1">Tax</td>
                                     <td class="col2"><asp:Label ID="lblSalesTaxAmount" Text='<%# Convert.ToDecimal(Eval("SalesTax")).ToString("C2")%>'
                                                                                    runat="server"></asp:Label></td>
                                    <td class="spacetd"></td>
                                  </tr>
                                  <tr>
                                  <td></td>
      	                            <td class="col1">Shipping Fee</td>
                                    <td class="col2"><asp:Label ID="lblShippingChargeAmount" Text='<%# Convert.ToDecimal(Eval("ShippingAmount")).ToString("C2")%>'
                                                                                  runat="server"></asp:Label></td>
                                    <td class="spacetd"></td>
                                  </tr>
  		                            <tr class="item-total">
  		                            <td></td>
      	                            <td class="col1">Grand Total</td>
                                    <td class="col2"><asp:Label ID="lblGrandTotalAmount" Text='<%# Convert.ToDecimal(Eval("TotalAmount")).ToString("C2")%>'
                                                                                    runat="server"></asp:Label></td>
                                    <td class="spacetd"></td>
                                  </tr>
  	                            </table>
  	                            <div class="warranty-page-btn">
  	                            <asp:LinkButton ID="lnkEditShipping" runat="server" title="Edit Shipping Info" CommandName="EditShippingInfo" CommandArgument='<%# Eval("OrderID")%>' class="small-gray-btn"><span>Edit Shipping</span></asp:LinkButton>
  	                            <asp:LinkButton ID="lnkbtnSaveChnages" runat="server" class="small-blue-btn" title="Save Changes" CommandName="SaveChanges" CommandArgument='<%# Eval("OrderID")%>'><span>Save Changes</span></asp:LinkButton>
  	                            <asp:LinkButton ID="lnkApprove" runat="server" class="small-gray-btn" title="Save & Approve"  CommandArgument='<%# Eval("OrderID") %>' CommandName="Approve"><span>Save & Approve</span></asp:LinkButton>
                                <a href="javascript:showOrderDetailsGV('Order-<%# Eval("OrderID") %>');" class="small-blue-btn" title="Close"><span>Close</span></a>
                                </div>
                               
                            </td>
                        </tr>
                     </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
               </asp:GridView>
        <div id="pagingtable"  runat="server" class="store-header cf">
                    <asp:LinkButton ID="lnkViewAllTop" runat="server" OnClick="lnkViewAll_Click" class="view-link">VIEW ALL</asp:LinkButton>
                    <%--START Top paging--%>
                    <div class="pagination alignright">
                        <asp:LinkButton ID="lnkbtnPrevious" runat="server" OnClick="lnkbtnPrevious_Click"
                            class="left-arrow"></asp:LinkButton>
                        <asp:DataList ID="dtlPaging" runat="server" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" OnItemCommand="dtlPaging_ItemCommand" OnItemDataBound="dtlPaging_ItemDataBound">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                    CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' OnClientClick="SetPageIndexActive(this);"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:LinkButton ID="lnkbtnNext" runat="server" OnClick="lnkbtnNext_Click" class="right-arrow"></asp:LinkButton>
                    </div>
                    <%--END Top paging--%>
                </div>
  </div>
   <%--Start Reason for Cancel Order Panel --%>
    <asp:Panel ID="pnlCancelOrder" runat="server" Style="display: none;">
        <div class="popup-outer popupouter-center" id="CancelOrder-popup">
            <div class="popupInner">
               <a id="a2" runat="server" href="javascript: void(0);" class="close-btn">Close</a>
                <div class="popup-block">
                    <h3>Reason for Cancelling</h3>
                    <label>                           
                        <textarea ID="txtReasonCode" rows="7" cols="7" runat="server" placeholder="Type reason for cancelling..."  class="default_title_text multiline-text"></textarea>
                    </label>                                                          
                    <div class="btn-block">                                           
                       <asp:LinkButton ID="lnkCancelOrder" runat="server" class="blue-btn" OnClick="lnkCancelOrder_Click">
                            Cancel Order</asp:LinkButton>
                    </div>                        
                </div>
                  <span class="popup-bot">&nbsp;</span> 
            </div>
        </div>
    </asp:Panel>
    <%--END Reason for Cancel Order Panel --%>
    <%--START Edit Shipping Panel --%>
    <asp:Panel ID="pnlEditShippingAddress" runat="server" Style="display: none;">
        <div class="popup-outer" id="register-block">
       <div class="popupInner">
            <div class="register-block">
                <a href="#" class="close-btn">Close</a>
                <div class="register-content">
                    <div class="register-header cf">
                        <h5>
                            In this popup you can edit the shipping information for this order. Once you make a change please click on the Save Changes button below to update the order shipping information.</h5>
                    </div>
                    <div class="cf">
                        <ul class="left-form">
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">First Name</span>
                                    <asp:TextBox ID="txtFirstName" runat="server" class="input-field first-field checkvalidation"
                                        TabIndex="5"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Last Name</span>
                                    <asp:TextBox ID="txtLastName" runat="server" class="input-field first-field checkvalidation"
                                        TabIndex="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLastName"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Company</span>
                                    <asp:TextBox ID="txtCompany" runat="server" class="input-field first-field checkvalidation" TabIndex="7"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCompany"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Address </span>
                                    <asp:TextBox ID="txtAddress1" runat="server" class="input-field" TabIndex="8"></asp:TextBox></label></li>
                               <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Suite/Apt.</span>
                                    <asp:TextBox ID="txtSuiteApt" runat="server" class="input-field" TabIndex="9"></asp:TextBox></label></li>
                            <li class="alignright">
                                    <label>
                                        <span class="lbl-txt">Country</span><span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlCountry" runat="server" class="default checkvalidation" TabIndex="10" 
											OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqddlCountry" runat="server" ControlToValidate="ddlCountry"
                                                Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                                ErrorMessage="*" InitialValue="0">
                                            </asp:RequiredFieldValidator>
                                        </span>
                                    </label>
                                </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">State </span> <span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlState" runat="server" TabIndex="10" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true" class="checkvalidation default">
                                            <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlState"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </span>
                                </label>
                            </li>
                             <li class="alignright">
                                <label>
                                    <span class="lbl-txt">City</span> <span class="select-drop medium-drop">
                                        <asp:DropDownList ID="ddlCity" runat="server" TabIndex="11" class="checkvalidation default">
                                            <asp:ListItem Value="0" Text="-select-"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlCity"
                                            Display="Dynamic" CssClass="error" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"
                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </span>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Zip Code</span>
                                    <asp:TextBox ID="txtZipCode" runat="server" class="input-field checkvalidation" TabIndex="11"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtZipCode"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                </label>
                            </li>
                            
                            <li class="alignright">
                                <label>
                                    <span class="lbl-txt">Email Address</span>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" class="input-field checkvalidation"
                                        TabIndex="13"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmailAddress"
                                        Display="Dynamic" CssClass="error" SetFocusOnError="True" ValidationGroup="Save"
                                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="rvEmail" Display="Dynamic" CssClass="error" runat="server"
                                        ControlToValidate="txtEmailAddress" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="Save" SetFocusOnError="True" ErrorMessage="*"></asp:RegularExpressionValidator>
                                </label>
                            </li>
                            <li class="alignleft">
                                <label>
                                    <span class="lbl-txt">Phone Number</span>
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" class="input-field" TabIndex="14"></asp:TextBox></label></li>
                        </ul>
                    </div>
                    <div class="reg-btn-block cf">
                    <a href="javascript:;" class="gray-home-btn" onclick="CloseShippingInfo();" ><span>Close</span></a>
                        <asp:LinkButton ID="lnkSaveShippingAddress" class="blue-home-btn sucess-link popup-openlink submit"
                            runat="server"  TabIndex="15" ValidationGroup="Save"
                            call="Save" OnClick="lnkSaveShippingAddress_Click"><span>Save Changes</span></asp:LinkButton>
                    </div>
                </div>
                </div>
            </div>
            </div>
    </asp:Panel>
    <%--END Edit Shipping Panel --%>
    <%--Start Confirmation Popup Panel --%>
    <div id="dvConfirmationMsg" style="display: none;">
        <div class="popup-outer popupouter-center" id="Confirmation-popup">
            <div class="popupInner bg-none">
                <div class="popup-block">
                    <h3>
                        Message</h3>
                    <div class="generalmsg-content">
                        <p id="pdMsg"></p>
                    </div>                                                       
                    <div class="btn-block">                                           
                        <a href="javascript:;" class="gray-btn cancel-btn" onclick="CloseConfirmation();" >No</a>
                         <asp:LinkButton ID="lnkbtnYes" runat="server" class="blue-btn" OnClick="lnkbtnYes_Click">
                            Yes</asp:LinkButton>
                    </div>                        
                </div>
                  <span class="popup-bot">&nbsp;</span> 
            </div>
        </div>
    </div>
    <%--END Reason for Cancel Order Panel --%>
    <%--  Hidden Field--%>
    <asp:HiddenField ID="hdnCancelOrderID" runat="server" />
    <asp:HiddenField ID="hdnMainStatus" runat="server" />
</section>
</asp:Content>
