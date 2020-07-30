<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SearchUsers.aspx.cs" Inherits="admin_searchusers" Title="Search Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" language="javascript">
        function pageLoad(sender, args) {
            {
                assigndesign();
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        $().ready(function() {
            $('#Gender option').each(function(i) {
                if ($("#ctl00_ContentPlaceHolder1_hdnGender").val() == $(this).val()) {
                    $(this).attr("selected", "selected");
                    $("#Gender").msDropDown({ mainCSS: 'dd2' });
                }
            });
            $('#Gender').change(function() {
                $("#ctl00_ContentPlaceHolder1_hdnGender").val($(this).val());
            });
            $('#Workgroup option').each(function(i) {
                if ($("#ctl00_ContentPlaceHolder1_hdnWorkgroup").val() == $(this).val()) {
                    $(this).attr("selected", "selected");
                    $("#Workgroup").msDropDown({ mainCSS: 'dd2' });
                }

            });
            $('#Workgroup').change(function() {
                $("#ctl00_ContentPlaceHolder1_hdnWorkgroup").val($(this).val());
            });
        });
       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form_pad">
        <div class="search_user_pad">
            <h4>
                Search Contacts</h4>
            <div style="text-align: left">
                <asp:Label ID="lblWorkgroupName" runat="server" CssClass="errormessage"></asp:Label>
            </div>
            <div>
                <table class="form_table">
                    <tr>
                        <td>
                            <div>
                                <div class="form_top_co">
                                    <span>&nbsp;</span></div>
                                <div class="form_box">
                                    <span class="input_label">First Name</span>
                                    <asp:TextBox ID="txtFirstName" TabIndex="1" runat="server" class="w_label"></asp:TextBox>
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
                                    <span class="input_label">Last Name</span>
                                    <asp:TextBox ID="txtLastname" TabIndex="2" runat="server" class="w_label"></asp:TextBox>
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
                                    <span class="input_label">Email Address</span>
                                    <asp:TextBox ID="txtEmailAddress" TabIndex="3" runat="server" class="w_label"></asp:TextBox>
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
                                    <span class="input_label">Employee #</span>
                                    <asp:TextBox ID="txtEmployeeId" TabIndex="4" runat="server" class="w_label"></asp:TextBox>
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
                                    <input id="hdnWorkgroup" type="hidden" value="0" runat="server" />
                                    <span class="input_label" style="width: 28%;">Workgroup</span>
                                    <label class="dropimg_width">
                                        <span class="form_box status_select label-sel-small">
                                            <div runat="server" id="ddlWorkgroup"></div>
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
                                    <input id="hdnGender" type="hidden" value="0" runat="server" />
                                    <span class="input_label" style="width: 28%;">Gender</span>
                                    <label class="dropimg_width">
                                        <span class="form_box status_select label-sel-small">
                                            <select tabindex="9" id="Gender" name="Gender">
                                                <option value="0">-select gender-</option>
                                                <%
                                                    Incentex.DA.LookupDA sGender = new Incentex.DA.LookupDA();
                                                    Incentex.BE.LookupBE sGenderBE = new Incentex.BE.LookupBE();
                                                    sGenderBE.SOperation = "selectall";
                                                    sGenderBE.iLookupCode = "Gender";

                                                    System.Data.DataSet dsGender = sGender.LookUp(sGenderBE);
                                                    for (int i = 0; i < dsGender.Tables[0].Rows.Count; i++)
                                                    {
                                                        string path = "../../Admin/Incentex_Used_Icons/" + dsGender.Tables[0].Rows[i]["sLookupIcon"].ToString();
                                                              
                                                                        
                                                %>
                                                <option value="<%=dsGender.Tables[0].Rows[i]["iLookupID"]%>" title="<%=path%>">
                                                    <%=dsGender.Tables[0].Rows[i]["sLookupName"].ToString()%></option>
                                                <%
                                                    }

                                                %>
                                            </select>
                                        </span>
                                        <div id="dvGender">
                                        </div>
                                    </label>
                                </div>
                                <div class="form_bot_co">
                                    <span>&nbsp;</span></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="botbtn centeralign">
                <asp:LinkButton CssClass="grey2_btn" ID="btnSearch" runat="server" TabIndex="7" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton>
                <asp:LinkButton CssClass="grey2_btn" ID="btnClear" runat="server" OnClick="btnClear_Click"><span>Clear</span></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
