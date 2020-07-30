<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UniformIssuanceStep5.aspx.cs" Inherits="admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1"
   %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script language="javascript" type="text/javascript">
 
     //change value in custom dropdown
  function pageLoad(sender, args) {
         {
             assigndesign();
         }
     }
     
    
    // modal popup for Not active feature
        function FeatureNotActive() {
            jAlert("Currently this feature is not active..", "Feature Not Active", function(RetVal) {
            if (RetVal) {
                }
            });
        }
    
    
</script>

     <script type="text/javascript">
         // pretty photo load event
         // refer for details http://forums.asp.net/p/1468161/3393543.aspx
        // function pageLoad() {
        //     $("a[rel^='prettyPhoto']").prettyPhoto();
       //  }  
 </script> 
    
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <mb:MenuUserControl ID="menuControl" runat="server" />
    
    <div class="form_pad">
    
        <div class="uniform_pad">
            <h4>
                Please review your selections and setting below to confirm.</h4>
            <div>
                <div class="black_top_co">
                    <span>&nbsp;</span></div>
                <div class="black_middle pad_none">
                    <div class="header_bg">
                        <div class="header_bgr title">
                            Department &amp; Workgroup</div>
                    </div>
                    <div class="issurance_detail">
                        <table>
                            <tr>
                                <td style="width: 40%;">
                                    Department :
                                </td>
                                <td style="width: 60%;">
                                   <%-- Flight Operations--%>
                                   <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Work Group :
                                </td>
                                <td>
                                   <%-- Pilots--%>
                                   <asp:Label ID="lblWorkGroup" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="header_bg">
                        <div class="header_bgr title">
                            Product Details & Issuances</div>
                    </div>
                    <div class="issurance_detail">
                        <div style="width: 100%;" class="alignleft">
                           
                            <asp:DataList ID="lstItems" runat="server" ondatabinding="lstItems_DataBinding" 
                                RepeatLayout="Table" onitemdatabound="lstItems_ItemDataBound"
                                Width="100%"
                                >
                     
                                <ItemTemplate>
                                <tr>
                                <td style="width: 50%;" align="left" valign="middle">
                                &nbsp;Association Issuance Type :
                                </td>
                                <td style="width: 50%;" align="left" valign="middle" colspan="2">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("sLookupName") %>'></asp:Label>
                                </td>
                                
                                </tr>
                                <tr>
                                <td style="width: 60%;" align="left" valign="middle">
                                &nbsp;Association Budget Amount ($) :
                                </td>
                                <td style="width: 40%;" align="left" valign="middle" colspan="2">
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("AssociationbudgetAmt") %>'></asp:Label>
                                </td>
                                
                                </tr>
                                
                                
                                    <tr>
                                    <td style="width: 20%;" align="left" valign="middle">
                                        &nbsp;Master Item # :
                                    </td>
                                    <td style="width: 50%;" align="left" valign="middle">
                                    <asp:HiddenField ID="hdnUniformIssuancePolicyItemID" Value=<%#Eval("UniformIssuancePolicyItemID") %> runat="server" />
                                    <asp:HiddenField id="hdnStoreProductid" runat="server" Value=<%#Eval("StoreProductid") %> />                                    
                                    <asp:HiddenField id="hdnMasterItemId" runat="server" Value=<%#Eval("MasterItemId") %>/>                                   
                                    <asp:Label ID="lblItem" runat="server"></asp:Label>
                                    <asp:Label ID="lblIssuance" runat="server" Text=<%# " |&nbsp;&nbsp;&nbsp;" +  Eval("Issuance") + " Issued" %>></asp:Label>
                                    </td>
                                   
                                    <td align="center">
                                        <asp:Image ID="imgPhoto" runat="server" Height="100" Width="100"/>
                                      </td>
                                </tr>
                                <tr>
                                <td style="width: 50%;" align="left" valign="middle">Payment Type :</td>
                                <td style="width: 50%;" align="left" valign="middle" colspan="2">
                                <asp:HiddenField ID="hdhPaymentOption" runat="server" Value=<%# Eval("PaymentOption") %>/>
                                <asp:Label ID="lblPaymentOption" runat="server"></asp:Label>
                                </td>
                               </tr>
                               </ItemTemplate>
                               <%--  <FooterTemplate>
                                    </table>
                                </FooterTemplate>--%>
                            </asp:DataList>
                                           
                        </div>
                   
                        <div class="alignnone">
                        </div>
                    </div>
                    <div class="header_bg">
                        <div class="header_bgr title">
                            Issuance Timeframe Rules</div>
                    </div>
                    <div class="issurance_detail">
                        <table>
                            <tr>
                                <td style="width: 40%;">
                                    Date of Hire :
                                </td>
                                <td style="width: 60%;">
                                    <span class="custom-checkbox true" runat="server" id="spChkDateOfHire">
                                        <%--<input type="checkbox" />--%>
                                        <asp:CheckBox ID="chkDateOfHire" Enabled="false" runat="server" />
                                        </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                 Issuance Period :
                                </td>
                                <td>
                                    <%--18 Months--%>
                                    <asp:Label ID="lblIssuancePeriod" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Expires After :
                                </td>
                                <td>
                                    <%--3 Months--%>
                                    <asp:Label ID="lblExpiresAfter" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="header_bg">
                        <div class="header_bgr title" id="divSystemReminderAlarms">
                            System Reminder Alarms</div>
                    </div>
                    <div class="issurance_detail">
                        <table>
                            <tr>
                                <td style="width: 40%;">
                                    1st Reminder Alarm :
                                </td>
                                <td style="width: 60%;">
                                    <asp:Label ID="lblReminder1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2nd Reminder Alarm :
                                </td>
                                <td>
                                    <asp:Label ID="lblReminder2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                  3rd Reminder Alarm :
                                </td>
                                <td>
                                    <asp:Label ID="lblReminder3" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   4th Reminder Alarm :
                                </td>
                                <td>
                                    <asp:Label ID="lblExpirationReminder" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel runat="server" ID="pnlCompanyPays">
                    <div class="header_bg">
                        <div class="header_bgr title" id="div1">
                            Billing Information</div>
                    </div>
                     <div class="issurance_detail">
                    <asp:DataList ID="dtBillingAddress" runat="server" ondatabinding="dtBillingAddress_DataBinding" 
                                RepeatLayout="Table" 
                                Width="100%"
                                >
                                 <ItemTemplate>
                                <tr>
                               <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 30%;" align="left" valign="middle">
                                    Company Name :  
                               </td>
                               <td style="width: 65%;" align="left" valign="middle">
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("BCompanyName")%>' ></asp:Label>
                                
                               </td>
                               
                               </tr>
                                <tr>
                               <td style="width: 10%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Name :
                               
                               </td>
                              
                               <td style="width: 80%;" align="left" valign="middle" >
                               <asp:Label ID="lbl" runat="server" Text='<%# Eval("FullName")%>'></asp:Label>
                               
                               </td>
                               
                               </tr>
                                <tr>
                               <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Address :
                               </td>
                               <td style="width: 85%;" align="left" valign="middle">
                               <asp:Label ID="lblAddress1" runat="server" Text='<%# Eval("Address1") + " ," + Eval("Address2")%>' ></asp:Label>
                                <asp:Label ID="lblAddress2" runat="server" Text='<%# Eval("CityName") + " ," + Eval("StateName") + " ," + Eval("CountryName")%>'></asp:Label>
                                 
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Zip Code :
                               </td>
                               <td style="width: 85%;" align="left" valign="middle">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ZipCode")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 20%;" align="left" valign="middle">
                               Mobile No :
                               </td>
                               <td  style="width: 75%;" align="left" valign="middle">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Mobile")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 20%;" align="left" valign="middle">
                               Telephone :
                               </td>
                               <td  style="width: 75%;" align="left" valign="middle">
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("Telephone")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Email :
                               </td>
                               <td  style="width: 85%;" align="left" valign="middle">
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                               </td>
                               </tr>
                    </ItemTemplate>
                    </asp:DataList>
                    </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="Panel1">
                    <div class="header_bg">
                        <div class="header_bgr title" id="div2">
                            Shipping Information</div>
                    </div>
                     <div class="issurance_detail">
                    <asp:DataList ID="dtShipping" runat="server" ondatabinding="dtShipping_DataBinding" 
                                RepeatLayout="Table" 
                                Width="100%"
                                >
                                 <ItemTemplate>
                                <tr>
                               <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 30%;" align="left" valign="middle">
                                    Company Name :  
                               </td>
                               <td style="width: 65%;" align="left" valign="middle">
                                <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("SCompanyName")%>' ></asp:Label>
                                
                               </td>
                               
                               </tr>
                                <tr>
                               <td style="width: 10%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Name :
                               
                               </td>
                              
                               <td style="width: 80%;" align="left" valign="middle" >
                               <asp:Label ID="lbl" runat="server" Text='<%# Eval("FullName")%>'></asp:Label>
                               
                               </td>
                               
                               </tr>
                                <tr>
                               <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Address :
                               </td>
                               <td style="width: 85%;" align="left" valign="middle">
                               <asp:Label ID="lblAddress1" runat="server" Text='<%# Eval("Address1") + " ," + Eval("Address2")%>' ></asp:Label>
                                <asp:Label ID="lblAddress2" runat="server" Text='<%# Eval("CityName") + " ," + Eval("StateName") + " ," + Eval("CountryName")%>'></asp:Label>
                                 
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Zip Code :
                               </td>
                               <td style="width: 85%;" align="left" valign="middle">
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ZipCode")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 20%;" align="left" valign="middle">
                               Mobile No :
                               </td>
                               <td  style="width: 75%;" align="left" valign="middle">
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Mobile")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 20%;" align="left" valign="middle">
                               Telephone :
                               </td>
                               <td  style="width: 75%;" align="left" valign="middle">
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("Telephone")%>'></asp:Label>
                               </td>
                               </tr>
                                <tr>
                                <td style="width: 5%;" align="left" valign="middle"></td>
                               <td style="width: 10%;" align="left" valign="middle">
                               Email :
                               </td>
                               <td  style="width: 85%;" align="left" valign="middle">
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                               </td>
                               </tr>
                    </ItemTemplate>
                    </asp:DataList>
                    </div>
                    </asp:Panel>
                </div>
                <div class="black_bot_co">
                    <span>&nbsp;</span></div>
            </div>
            <div class="uniform_grey_btn_pad">
              
                <asp:LinkButton ID="lnkEditDetails" runat="server" 
                    CssClass="grey2_btn alignleft" onclick="lnkEditDetails_Click" >
                    <span>Edit Details</span>
                </asp:LinkButton>

               
                    <asp:LinkButton ID="lnkProcess" runat="server"
                    class="grey2_btn alignright" OnClick="lnkProcess_Click"
                    >
                        <span>Process Issuance Policy</span>
                    </asp:LinkButton>
            </div>
        </div>
        <div class="spacer25">
        </div>
        <div class="bot_alert prev">
         
            <asp:LinkButton ID="lnkPrev" runat="server" title="Back" OnClick="lnkPrev_Click">Back</asp:LinkButton>
            
            </div>
    </div>
</asp:Content>
