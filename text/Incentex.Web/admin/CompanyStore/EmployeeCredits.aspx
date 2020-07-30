<%@ Page Language="C#" MasterPageFile="~/admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="EmployeeCredits.aspx.cs" Inherits="admin_CompanyStore_EmployeeCredits" %>

<%@ Register Src="~/admin/UserControl/MenuUserControl.ascx" TagName="MenuUserControl"
    TagPrefix="mb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <at:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </at:ToolkitScriptManager>

    <script type="text/javascript">
        // Let's use a lowercase function name to keep with JavaScript conventions
        function selectAll(invoker) {
            // Since ASP.NET checkboxes are really HTML input elements
            //  let's get all the inputs
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                // Filter through the input types looking for checkboxes
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }

        // check if value is numeric in textbox
        function CheckNum(id) {
            var txt = document.getElementById(id);
            if (!IsNumeric(txt.value)) {
                alert("Please enter numeric value");
                txt.value = "";
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

        
    </script>

    <%-- <mb:MenuUserControl ID="menucontrol" runat="server" />
   --%>
    <div class="form_pad">
        <asp:UpdateProgress runat="server" ID="uprogressPGrid" DisplayAfter="1" AssociatedUpdatePanelID="upPanel">
            <ProgressTemplate>
                <div id="updateProgressBackgroundFilter" class="updateProgressBackgroundFilter">
                </div>
                <div class="updateProgressDiv">
                    <img alt="Loading" src="../../Images/ajax-loader-large.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <div style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="errormessage"></asp:Label>
                </div>
                <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="ord_header"
                    CssClass="orderreturn_box" GridLines="None" RowStyle-CssClass="ord_content" OnPageIndexChanging="gvEmployee_PageIndexChanging"
                    OnRowCommand="gvEmployee_RowCommand" OnRowDataBound="gvEmployee_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCompanyEmployeeID" Text='<%# Eval("CompanyEmployeeID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                            <HeaderTemplate>
                                <span>
                                    <asp:CheckBox ID="cbSelectAll" runat="server" OnClick="selectAll(this)" />&nbsp;</span>
                                <div class="corner">
                                    <span class="ord_headtop_cl"></span><span class="ord_headbot_cl"></span>
                                </div>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="centeralign" />
                            <ItemTemplate>
                                <span class="first">
                                    <asp:CheckBox ID="chkSelectUser" runat="server" />&nbsp; </span>
                                <div class="corner">
                                    <span class="ord_greytop_cl"></span><span class="ord_greybot_cl"></span>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="5%" CssClass="g_box centeralign" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="EmployeeID">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmployeeNo" runat="server" CommandArgument="EmployeeID"
                                    CommandName="Sort"><span>Emp#</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <asp:HiddenField ID="hdnStartingCredit" runat="server" />
                                    <asp:HiddenField ID="hdnAnniversary" runat="server" />
                                    <asp:LinkButton ID="lnkEmployeeNo" runat="server" CommandName="EditEmp" Text='<%# Eval("EmployeeID") %>'
                                        CommandArgument='<%# Eval("CompanyEmployeeID") %>'>
                                    </asp:LinkButton>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="FullName">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCusName" runat="server" CommandArgument="FullName" CommandName="Sort"><span>Employee Name</span></asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderEmployeeName" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span>
                                    <%#(Eval("FullName").ToString().Length > 24) ? Eval("FullName").ToString().Substring(0, 24) + "..." : Eval("FullName")%>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Workgroup">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnCountry" runat="server" CommandArgument="Workgroup" CommandName="Sort"><span class="white_co">Workgroup</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkgroup" Text='<%#(Eval("Workgroup").ToString().Length > 8) ? Eval("Workgroup").ToString().Substring(0, 8) + "..." : Eval("Workgroup")%>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="State">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnState" runat="server" CommandArgument="State" CommandName="Sort">
                                <span>State</span>
                                </asp:LinkButton>
                                <asp:PlaceHolder ID="placeholderState" runat="server"></asp:PlaceHolder>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStationManager" Text='<%#(Eval("State").ToString().Length > 9) ? Eval("State").ToString().Substring(0, 9) + "..." : Eval("State") + "&nbsp;" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="Email">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnEmail" runat="server" CommandArgument="Email" CommandName="Sort">
                                    <span class="white_co">Email</span></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span><a href='<%# "mailto:" + Eval("Email") %>'>
                                    <%# Eval("Email") %>
                                </a></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="HirerdDate">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkbtnHirerdDate" runat="server" CommandArgument="HirerdDate"
                                    CommandName="Sort">
                                <span>Hire Date</span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblHirerdDater" Text='<%#Eval("HirerdDate")!= DBNull.Value ? Convert.ToDateTime(Eval("HirerdDate")).ToShortDateString() : "--" %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="g_box" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="StratingCreditAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkStratingCreditAmount" runat="server" CommandArgument="StratingCreditAmount"
                                    CommandName="Sort">
                                <span class="white_co">Starting Credit($) </span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">$
                                    <asp:TextBox ID="txtStratingCreditAmount" runat="server" Width="50px" Style="background-color: #303030;
                                        border: medium none; color: #ffffff; width: 70px; padding: 2px" onchange="CheckNum(this.id)"></asp:TextBox>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="StratingCreditAmount">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkAnniversaryCreditAmount" runat="server" CommandArgument="StratingCreditAmount"
                                    CommandName="Sort">
                                <span class="white_co">Anniversary Credit($) </span>
                                </asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="btn_space">$
                                    <asp:TextBox ID="txtAnniversaryCreditAmount" runat="server" Width="50px" Style="background-color: #303030;
                                        border: medium none; color: #ffffff; width: 50px; padding: 2px" onchange="CheckNum(this.id)"
                                        Enabled="false"></asp:TextBox>&nbsp;
                                    <asp:LinkButton ID="lnkSaveAnnive" CommandName="saveAnniversary" CommandArgument='<%# Eval("CompanyEmployeeID") %>'
                                        runat="server" OnClientClick="return confirm('Are you sure you want to save record?');"
                                        ToolTip="Update Anniversary credits of this user!"><img style="height:25px;width:25px;" src="../../images/save-information-icon.png" alt=""  /></asp:LinkButton>
                                </span>
                            </ItemTemplate>
                            <ItemStyle CssClass="b_box" HorizontalAlign="Center" Width="16%" />
                        </asp:TemplateField>
                      
                    </Columns>
                </asp:GridView>
                <div>
                    <div>
                        <div class="companylist_botbtn alignleft">
                            <asp:LinkButton ID="lnkSave" runat="server" class="grey_btn" OnClick="lnkSave_Click"><span>Save Credit</span></asp:LinkButton>
                        </div>
                        <div class="companylist_botbtn alignleft">
                            <asp:LinkButton ID="lnkBtnAddEmployee" OnClick="lnkBtnAddEmployee_Click" runat="server"
                                class="grey_btn"><span>Add Employee</span></asp:LinkButton>
                        </div>
                        <div class="alignright pagging" id="dvPaging" runat="server">
                            <asp:LinkButton ID="lnkbtnPrevious" class="prevb" runat="server" OnClick="lnkbtnPrevious_Click"> 
                            </asp:LinkButton>
                            <span>
                                <asp:DataList ID="dtlViewEmployee" runat="server" CellPadding="1" CellSpacing="1"
                                    OnItemCommand="dtlViewEmployee_ItemCommand" OnItemDataBound="dtlViewEmployee_ItemDataBound"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                            CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </span>
                            <asp:LinkButton ID="lnkbtnNext" class="nextb" runat="server" OnClick="lnkbtnNext_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
