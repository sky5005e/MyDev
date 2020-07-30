<%@ Page Title="" Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="UserPages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%--<iframe width="560" height="315" src="http://10.2.5.5:91/UserPages/Mirami feat. LayZee - Summer Dreams (official video).avi"  frameborder="0"></iframe>--%>

<asp:PlaceHolder ID="flashPlaceHolder" runat="server"></asp:PlaceHolder>
<script>
    $(document).ready(function() {

        $("#Checkbox1").on('ifChecked', function(event) {
            alert(event.type + ' callback 1');
        });
    });
</script>
<script type="text/javascript">
    $(document).ready(function() {
        if ($("#Checkbox1").length > 0) {
            alert('hi');
        }
        else {

            $('input').iCheck({

                checkboxClass: 'icheckbox_flat',
                radioClass: 'iradio_flat'
            });
        }

        //        $("#vidsrc > source").attr("src", 'http://10.2.5.5:91/NewDesign/UserPages/Aaya Bihu jhoom ke....tngsss.mp4');
        //        var v = document.getElementById("vidsrc");
        //        v.load();
        //        v.play();


    });
    function click1() {
        alert('click 1');
    }

    function click2() {
        alert('click 2');
    }
</script>
<style type="text/css">
		ul {
			margin:0;
			padding:0;
			list-style-type:none;
			min-width:200px;
		}
 
		ul#navigation {
			float:left;
		}
 
		ul#navigation li {
			float:left;
			border:1px black solid;
			min-width:200px;
			background : Gray;
		}
 
		ul.sub_navigation {
			position:absolute;
			display:none;
			background: Gray;
			z-index:99;
		}
 
		ul.sub_navigation li {
			clear:both;
			background: Gray;
		}
 
		a,
		a:active,
		a:visited {
			display:block;
			padding:10px;
		}

/*New Style Of UI*/
#form-elm{ width:980px; margin:0px auto; border:1px solid #ccc; padding:15px;}
#form-elm h2{ font-size:16px; margin:5px 0px; line-height:25px; padding-top:15px; clear:both;}
.apple_checkbox .label_check{ padding-left:0px;}
.checkbox .label_check{ padding-left:0px;}
</style>
	<%--<script type="text/javascript" src="jquery.js"></script>--%>
	<%--<script type="text/javascript">
	    // Wait for the page and all the DOM to be fully loaded
	    $('body').ready(function() {

	        // Add the 'hover' event listener to our drop down class
	        $('.dropdown').hover(function() {
	            // When the event is triggered, grab the current element 'this' and
	            // find it's children '.sub_navigation' and display/hide them
	            $(this).find('.sub_navigation').slideToggle();
	        });
	    });
	</script>--%>
	<%--<ul id="navigation">
	<li class="dropdown"><a href="#">Dropdown</a>
		<ul class="sub_navigation">
			<li><a href="#" onclick="click2();">Sub Navigation 1</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 2</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
			<li><a href="#" onclick="click2();">Sub Navigation 3</a></li>
		</ul>
	</li>
</ul>--%>

numeric pad 
<asp:TextBox ID="txttest" runat="server"  type="text" pattern="\d*"></asp:TextBox>
<input type="text" pattern="\d*">


<div id="form-elm">
<h2>Large Dropdown </h2>
<div class="select-drop mediumLarge-drop">
    <select class="default" name="station-code">
        <option value="">Select...</option>
		<option value="">Registration State 1</option>
        <option value="">Registration State 2</option>
        <option value="">Registration State 3</option>
	</select>
</div>

<h2>Midium Dropdown </h2>
<div class="select-drop medium-drop">
    <select class="default" name="station-code">
        <option value="">Select...</option>
		<option value="">Registration State 1</option>
        <option value="">Registration State 2</option>
        <option value="">Registration State 3</option>
	</select>
</div>

<h2>Month Dropdown </h2>
<div class="select-drop month-drop">
    <select class="default" name="station-code">
        <option value="">Select...</option>
		<option value="">Registration State 1</option>
        <option value="">Registration State 2</option>
        <option value="">Registration State 3</option>
	</select>
</div>


<h2>Day Dropdown </h2>
<div class="select-drop cmn-drop">
    <select class="default" name="station-code">
        <option value="">Day</option>
		<option value="">Registration State 1</option>
        <option value="">Registration State 2</option>
        <option value="">Registration State 3</option>
	</select>
</div>

<h2>Cart Dropdown </h2>
<div class="cart-dropin">
    <select class="default" name="station-code">
        <option value="">Day</option>
		<option value="">Registration State 1</option>
        <option value="">Registration State 2</option>
        <option value="">Registration State 3</option>
	</select>
</div>

<h2>Cart Dropdown </h2>
<div class="table-drop">
    <select class="default" name="station-code">
        <option value="">Day</option>
		<option value="">State 1</option>
        <option value="">State 2</option>
        <option value="">State 3</option>
	</select>
</div>

<h2>Small Blue Button</h2>
<a class="small-blue-btn" href="#"><span>POST SUMMARY</span></a>

<h2>Blue Button</h2>
<a class="blue-btn" href="#"><span>POST SUMMARY</span></a>

<h2>Button Save Cancel</h2>
<div class="asset-btn-block last">
    <a class="small-gray-btn" href="#"><span>Cancel</span></a>
    <a class="small-blue-btn" title="SAVE" href="#"><span>SAVE</span></a>
</div>


<h2>CheckBox On Off</h2>
<div class="apple_checkbox">
<label class="label_check"><input  type="checkbox" />&nbsp;</label>
</div>

<h2>CheckBox</h2>
<div class="checkbox">
<label class="label_check"><input  type="checkbox" />&nbsp;</label>
</div>







<div style="clear:both"></div>
</div>



    <br />
    <div style="height:950px;"> &nbps; </div>

    <%--<iframe src="http://www.streamer247.com/star3.php" width="700" height="600" />--%>

    <%--<iframe src="http://www.streamer247.com/star3.php" width="700" height="600" />--%>
    <iframe src="http://cricpower.com/embed.php" width="650" height="450" />

    <%--<iframe src="http://www.youtube.com/v/AzLil8ImkUw?hl=en_US&amp;version=3" width="600" height="400" />--%>
   <%-- <iframe src="http://www.youtube.com/v/AzLil8ImkUw?hl=en_US&amp;version=3" width="600" height="400"/>--%>
    <%--<object width="420" height="315"><param name="movie" value="//www.youtube.com/v/NiqRvdCzFqE?hl=en_US&amp;version=3"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="//www.youtube.com/v/NiqRvdCzFqE?hl=en_US&amp;version=3" type="application/x-shockwave-flash" width="420" height="315" allowscriptaccess="always" allowfullscreen="true"></embed></object>
    <br />--%>
     
   <%-- <video id="vidsrc" width="320" height="240" controls="true" preload="auto" >
  <source type="video/mp4">
Your browser does not support video
</video>--%>
<%--<label for="Checkbox1"><input id="Checkbox1" class="icheckbox_flat" type="checkbox" name="remember[1]"/> Remember me</label>
<label for="remember1"><input id="remember1" class="icheckbox_flat" type="checkbox" name="remember[1]" /> Remember me</label>
<label for="remember2"><input id="remember2" class="icheckbox_flat" type="checkbox" name="remember[2]" checked /> Remember me checked</label>
<label for="remember3"><input id="remember3" class="icheckbox_flat" type="checkbox" name="remember[3]" disabled /> Remember me disabled</label>

<label for="radio1"><input id="radio1" class="iradio_flat" type="radio" name="radio[1]" /> Remember me</label>
<label for="radio2"><input id="radio2" class="iradio_flat" type="radio" name="radio[2]" checked /> Remember me checked</label>
<label for="radio3"><input id="radio3" class="iradio_flat" type="radio" name="radio[3]" disabled /> Remember me disabled</label>--%>
</asp:Content>

