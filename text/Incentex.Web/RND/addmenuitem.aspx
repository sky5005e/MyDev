<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addmenuitem.aspx.cs" Inherits="RND_addmenuitem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../CSS/prettyPhoto.css" type="text/css" charset="utf-8" />
    <script type="text/javascript" src="../JS/jquery.js"></script>
    <script type="text/javascript" src="../JS/general.js"></script>
    <script type="text/javascript" src="../JS/jquery-ui.min.js"></script>
    <script src="../JS/jquery.prettyPhoto.js" type="text/javascript" charset="utf-8"></script>
    <script language="javascript" type="text/javascript">
        function funclick() {

            alert("clicked");
        
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper" class="clearfix">
        <div id="header" class=" clearfix">
            <div class="top_logo_section">
                <h1 id="logo">
                    <a href="index.html" title="Incentex">Incentex</a></h1>
            </div>
            <div class="black_round_top">
                <span></span>
            </div>
            <div class="header_section black_round_middle inner_top_middle">
                <div class="banner innner_banner">
                    <h2>
                        <img src="../Images/agent-img-co.gif" alt="" /></h2>
                    <a href="#" class="btn_red" title="Exit System"><span>Exit System </span></a>
                </div>
            </div>
        </div>
        <div id="content" class="black_round_middle">
            <div class="black_round_box">
                <div class="black2_round_top">
                    <span></span>
                </div>
                <div class="black2_round_middle">
                    <div class="header_bg">
                        <div class="header_bgr">
                            <span class="title alignleft">Proof Status</span> <a href="#" class="grey_btn alignright"
                                title="Go to Main Menu"><span>Go to Main Menu</span></a> <span class="date alignright">
                                    November 17, 2009</span>
                            <div class="alignnone">
                                &nbsp;</div>
                        </div>
                    </div>
                    <div class="alignnone">
                        &nbsp;</div>
                    <div class="form_pad">
                        <div>
                            <table class="form_table">
                                <tr>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <img src="../Images/proof-supplier-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Sent from Supplier') {this.value = '';}"
                                                        onblur="if (this.value == ''){this.value = 'Proof Sent from Supplier';}" value="Proof Sent from Supplier"
                                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="images/close-btn.png"
                                                            alt="" /></a></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <img src="../Images/proof-arrived-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Arrived at Office') {this.value = '';}"
                                                        onblur="if (this.value == ''){this.value = 'Proof Arrived at Office';}" value="Proof Arrived at Office"
                                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="images/close-btn.png"
                                                            alt="" /></a></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    <td class="formtd_r">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <img src="../Images/proof-sent-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Sent to Customer') {this.value = '';}"
                                                        onblur="if (this.value == ''){this.value = 'Proof Sent to Customer';}" value="Proof Sent to Customer"
                                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="images/close-btn.png"
                                                            alt="" /></a></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="spacer10" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <img src="../Images/new-proofrequired-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'New Proof Required') {this.value = '';}"
                                                        onblur="if (this.value == ''){this.value = 'New Proof Required';}" value="New Proof Required"
                                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="images/close-btn.png"
                                                            alt="" /></a></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    <td class="formtd">
                                        <div>
                                            <div class="form_top_co">
                                                <span>&nbsp;</span></div>
                                            <div class="form_box clearfix dropdown_search">
                                                <span class="alignleft status_detail">
                                                    <img src="../Images/proof-approved-img.jpg" alt="" /><input type="text" onfocus="if (this.value == 'Proof Approved') {this.value = '';}"
                                                        onblur="if (this.value == ''){this.value = 'Proof Approved';}" value="Proof Approved"
                                                        class="proof_input" /></span><span class="alignright"><a href="#"><img src="images/close-btn.png"
                                                            alt="" /></a></span>
                                            </div>
                                            <div class="form_bot_co">
                                                <span>&nbsp;</span></div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="spacer10" colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td class="gallery" colspan="3">
                                        <a href="#inline_popup1" class="grey2_btn alignright" rel="prettyPhoto[inline]"><span>
                                            + Add</span></a>
                                    </td>
                                </tr>
                                <div id="inline_popup1" style="display: none;">
                                    <div class="login_top">
                                        <h2>
                                            Add menu item</h2>
                                        <div class="input_bg clearfix">
                                            <label>
                                                Text
                                            </label>
                                            <asp:TextBox ID="txtPriorityName" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="input_bg clearfix">
                                            <label>
                                                Icon</label>
                                            <input type="file" id="flFile" runat="server" />
                                        </div>
                                        <div>&nbsp;</div>
                                        <div class="alignleft clearfix">
                                            <a onclick="javascript:funclick();" class="btn_gray"><span>Add</span></a>
                                        </div>
                                    </div>
                                </div>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="black2_round_bottom">
                    <span></span>
                </div>
                <div class="alignnone">
                    &nbsp;</div>
            </div>
        </div>
        <div id="footer">
            <div class="black_round_bottom">
                <span></span>
            </div>
            <div class="copyright">
                Copyright &copy; 2010 Incentex. All rights reserved
            </div>
        </div>
    </div>
    </form>
</body>
</html>
