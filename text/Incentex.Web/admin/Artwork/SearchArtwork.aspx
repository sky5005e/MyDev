<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="SearchArtwork.aspx.cs" Inherits="admin_Artwork_SearchArtwork" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </at:ToolkitScriptManager>
    <style type="text/css">
        .custom-sel select 
        {
            position: relative;
            margin-bottom: -8px;
        }
        .radiobutton input
        {
        	margin-right:5px;
        }
        .radiobutton label
        {
        	margin-right:20px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $.get('../../JS/JQuery/validationMessages/commonValidationMsg.xml', function(xml) {
                objValMsg = $.xml2json(xml);
                $("#aspnetForm").validate({
                    rules: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlProduct: { NotequalTo: "0" },
                        ctl00$ContentPlaceHolder1$ddlSubcategory: { NotequalTo: "0" }
                    },
                    messages: {
                        ctl00$ContentPlaceHolder1$ddlCompany: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "customer") },
                        ctl00$ContentPlaceHolder1$ddlProduct: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "product") },
                        ctl00$ContentPlaceHolder1$ddlSubcategory: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "sub category") },
                        ctl00$ContentPlaceHolder1$ddlStoreStatus: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "file category") }
                    },
                    errorPlacement: function(error, element) {
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlCompany")
                            error.insertAfter("#dvCompany");
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlProduct")
                            error.insertAfter("#dvProduct");
                        if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlSubcategory")
                            error.insertAfter("#dvSubCategory");
                        else if (element.attr("name") == "ctl00$ContentPlaceHolder1$ddlStoreStatus")
                            error.insertAfter("#dvFC");
                        else
                            error.insertAfter(element);
                    }
                });
            });

            // rb ProductCategory
            $("#ctl00_ContentPlaceHolder1_rbProductCategory").click(function() {
            $("#ctl00_ContentPlaceHolder1_ddlStoreStatus").rules("remove");
            $("#ctl00_ContentPlaceHolder1_ddlCompany").rules("remove");
                $("#ctl00_ContentPlaceHolder1_ddlProduct").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "product") }
                });
                $("#ctl00_ContentPlaceHolder1_ddlSubcategory").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "sub category") }
                });

                if ($("#aspnetForm").valid()) {
                    return true;

                }
                else {
                    return false;
                }
            });
            // rb ArtCategory click
            $("#ctl00_ContentPlaceHolder1_rbArtCategory").click(function() {
                $("#ctl00_ContentPlaceHolder1_ddlProduct").rules("remove");
                $("#ctl00_ContentPlaceHolder1_ddlSubcategory").rules("remove");
                $("#ctl00_ContentPlaceHolder1_ddlStoreStatus").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "file category") }
                });
                $("#ctl00_ContentPlaceHolder1_ddlCompany").rules("add", {
                    NotequalTo: "0",
                    messages: { NotequalTo: replaceMessageString(objValMsg, "NotEqualTo", "company") }
                });

                if ($("#aspnetForm").valid()) {
                    return true;
                }
                else {
                    return false;
                }

            });

            $("#<%=lnkBtnSearchInfo.ClientID %>").click(function() {
                return $('#aspnetForm').valid();
            });

        });        
    </script>

    <div class="form_pad select_box_pad" style="width: 390px;">
        <div class="form_table">
            <table>
                <tr>
                    <td class="radiobutton">
                        <asp:RadioButton ID="rbProductCategory" runat="server" Text="Product Image" GroupName="ImgCategories"
                            AutoPostBack="True" OnCheckedChanged="rbProductCategory_CheckedChanged" />
                        <asp:RadioButton ID="rbArtCategory" runat="server" Text="Art Library Image" GroupName="ImgCategories"
                            AutoPostBack="True" OnCheckedChanged="rbArtCategory_CheckedChanged" />
                    </td>
                </tr>
                <tr id="trCompany" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Company Name</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlCompany" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvCompany">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trStoreStatus" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Store</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlStoreStatus" onchange="pageLoad(this,value);" runat="server"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvFC">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trFileName" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">File Name</span>
                                <asp:TextBox ID="txtFileName" TabIndex="2" runat="server" CssClass="w_label"></asp:TextBox>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trProduct" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Product Category</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvProduct">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trSubCategory" runat="server">
                    <td>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box">
                                <span class="input_label" style="width: 30%;">Sub Category</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlSubcategory" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                                <div id="dvSubCategory">
                                </div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr id="trWorkgroup" runat="server">
                    <td>
                       <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box shipmax_in">
                                <span class="input_label" style="width: 30%">Workgroup</span> <span class="custom-sel label-sel">
                                    <asp:DropDownList ID="ddlWorkgroup" runat="server" TabIndex="6" onchange="pageLoad(this,value);">
                                    </asp:DropDownList>
                                </span>
                                 <div id="dvWorkgroup" ></div>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="centeralign">
                        <asp:LinkButton ID="lnkBtnSearchInfo" class="grey2_btn" runat="server" ToolTip="Search files"
                            OnClick="lnkBtnSearchInfo_Click"><span>Search</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
