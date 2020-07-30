<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="DropDownMenu.aspx.cs" Inherits="AssetManagement_DropDownMenu" Title="Drop Down Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:LinkButton ID="lnkJobCode" class="gredient_btnMainPage" title="Job Code" runat="server"
                        PostBackUrl="~/AssetManagement/vLookupJobCode.aspx">
                <img src="../admin/Incentex_Used_Icons/Jobcode.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Job Code             
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkJobSubCode" class="gredient_btnMainPage" title="Job Sub Code"
                        runat="server" PostBackUrl="~/AssetManagement/vLookupJobSubCode.aspx">
                <img src="../admin/Incentex_Used_Icons/jobsubcode_icon.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Job Sub Code             
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkProductCategory" class="gredient_btnMainPage" title="Product Category"
                        runat="server" PostBackUrl="~/AssetManagement/vLookupCategory.aspx">
                <img src="../admin/Incentex_Used_Icons/Productcategory_icon.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Product Category            
                </span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkProductSubCategory" class="gredient_btnMainPage" title="Product Sub Category"
                        runat="server" PostBackUrl="~/AssetManagement/vLookupProductSubCategory.aspx">
                <img src="../admin/Incentex_Used_Icons/Productsubcategory_icon.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Product Sub Category            
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="RepairReason" class="gredient_btnMainPage" ToolTip="Reason For Repair"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/AddNewEquipment.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Reason For Repair           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="RepairStatus" class="gredient_btnMainPage" ToolTip="Repair Order Status"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/Incentex-managesupplierpartner.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Repair Order Status           
                </span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="EquipmentModel" class="gredient_btnMainPage" ToolTip="Equipment Model"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/eqipment-model.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                 Equipment Model           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="OwnedBy" class="gredient_btnMainPage" ToolTip="Owned By" runat="server"
                        OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/owned-by.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Owned By           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="PowerSource" class="gredient_btnMainPage" ToolTip="Power Source"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/power-source.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Power Source          
                </span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="NewOrRefurbished" class="gredient_btnMainPage" ToolTip="New or Refurbished"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/new-refurbished.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                 New or Refurbished           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="PurchaseMethod" class="gredient_btnMainPage" ToolTip="Purchase Method"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/purchase-method.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Purchase Method           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="AircraftType" class="gredient_btnMainPage" ToolTip="Aircraft Type"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/aircrafttype.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Aircraft Type          
                </span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="MaxAircraftWeight" class="gredient_btnMainPage" ToolTip="Max Aircraft Weight"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/max-weight.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                 Max Aircraft Weight           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="EquipmentLife" class="gredient_btnMainPage" ToolTip="Equipment Life"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/equipment-life.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                 Equipment Life           
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="WarrantyBy" class="gredient_btnMainPage" ToolTip="Warranty By"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                        <img src="../admin/Incentex_Used_Icons/equipment-life.png" alt="World-Link System Control" />
                        <span style="width:200px;">               
                         Warranty By 
                        </span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:LinkButton ID="WarrantyTerms" class="gredient_btnMainPage" ToolTip="Warranty Terms"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/aircrafttype.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Warranty Terms
                </span>
                    </asp:LinkButton>
                </td>
                <td >
                    <asp:LinkButton ID="PurchaseConditions" class="gredient_btnMainPage" ToolTip="Purchase Conditions"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/aircrafttype.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Purchase Conditions
                </span>
                    </asp:LinkButton>
                </td>
                <td>
                 <asp:LinkButton ID="BrakeSystem" class="gredient_btnMainPage" ToolTip="Brake System"
                        runat="server" OnClick="lnkbtnDropDown_Click">
                <img src="../admin/Incentex_Used_Icons/aircrafttype.png" alt="World-Link System Control" />
                <span style="width:200px;">               
                  Brake System
                </span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
