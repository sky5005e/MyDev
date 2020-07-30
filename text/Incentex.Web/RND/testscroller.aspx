<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="testscroller.aspx.cs" Inherits="RND_testscroller" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        $(function() {
            scrolltextarea(".scrollme", "#Scrolltop1", "#ScrollBottom1");
            scrolltextarea(".scrollme1", "#A1", "#A2");
            //Use below function for scrollable with textmode ="Multiline"
            //scrolltextarea("cssclass of a textbox", "id of a anchor tag for upper image", "id of a anchor tag for bottom image");
            scrolltextarea(".scrollme3", "#Scrolltop3", "#ScrollBottom3");

            

        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table class="form_table">
            <tr>
                <td class="formtd">
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">Company Name</span>
                                        <input type="text" class="w_label" />
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
                                        <span class="input_label">State/Province</span>
                                        <input type="text" class="w_label" />
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
                                        <span class="input_label">Telephone</span>
                                        <input type="text" class="w_label" />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="formtd">
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box employeeedit_text clearfix">
                                        <span class="input_label alignleft">Address</span>
                                        <div class="textarea_box alignright">
                                            <div class="scrollbar">
                                                <a href="#scroll" id="Scrolltop1" class="scrolltop"></a><a href="#scroll" id="ScrollBottom1"
                                                    class="scrollbottom"></a>
                                            </div>
                                            <asp:TextBox ID="txtAdrress" Enabled="false" TextMode="MultiLine" CssClass="scrollme" runat="server"></asp:TextBox>
                                        </div>
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
                                    <div class="form_box employeeedit_text clearfix">
                                        <span class="input_label alignleft">Address1</span>
                                        <div class="textarea_box alignright">
                                            <div class="scrollbar">
                                                <a href="#scroll" id="A1" class="scrolltop"></a><a href="#scroll" id="A2" class="scrollbottom">
                                                </a>
                                            </div>
                                            <asp:TextBox ID="TextBox1" TextMode="MultiLine" CssClass="scrollme1" runat="server"></asp:TextBox>
                                        </div>
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
                                        <span class="input_label">Web-site</span>
                                        <input type="text" class="w_label" />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="formtd_r">
                    <table>
                        <tr>
                            <td>
                                <div>
                                    <div class="form_top_co">
                                        <span>&nbsp;</span></div>
                                    <div class="form_box">
                                        <span class="input_label">City</span>
                                        <input type="text" class="w_label" />
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
                                        <span class="input_label">Country</span>
                                        <input type="text" class="w_label" />
                                    </div>
                                    <div class="form_bot_co">
                                        <span>&nbsp;</span></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table class="form_table">
            <tr>
                <td>
                    <div>
                        <div class="form_top_co">
                            <span>&nbsp;</span></div>
                        <div class="form_box taxt_area clearfix">
                            <span class="input_label alignleft">Notes/History</span>
                            <div class="textarea_box alignright">
                                <div class="scrollbar">
                                    <a href="#scroll" id="Scrolltop3" class="scrolltop"></a><a href="#scroll" id="ScrollBottom3"
                                        class="scrollbottom"></a>
                                </div>
                                <asp:TextBox ID="txtScroll3" TextMode="MultiLine" runat="server" CssClass="scrollme3" Text="Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Cras metus. In gravida. Nulla vel justo in magna adipiscing vulputate. Fusce nunc tortor, facilisis nec, posuere sit amet, venenatis et, tortor. Proin turpis. Maecenas quis dolor lobortis nulla iaculis tempor. Nulla feugiat, dui sed sagittis dapibus, lacus augue imperdiet enim, id varius velit eros in dolor. Integer felis quam, imperdiet cursus, laoreet non, consequat a, enim."></asp:TextBox>
                                
                            </div>
                        </div>
                        <div class="form_bot_co">
                            <span>&nbsp;</span></div>
                    </div>
                </td>
            </tr>
          
        </table>
    </div>
</asp:Content>
