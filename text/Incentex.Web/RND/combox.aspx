<%@ Page Language="C#" AutoEventWireup="true" CodeFile="combox.aspx.cs" Inherits="combox" %>

<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    

    <script language="javascript" src="../msdropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../JS/JQuery/jquery.validate.js" type="text/javascript" language="javascript"></script>

    <script language="javascript" src="../msdropdown/js/jquery.dd.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../msdropdown/dd.css" />
    <title>Untitled Page</title>
</head>
<body>

    <script language="javascript" type="text/javascript">


        $(document).ready(function() {
            $('#form1').validate({
                rules: {
                    websites3: { NotequalTo: "0" }
                },
                messages: {
                    websites3: { NotequalTo: "hi this is ankit" }
                },
                errorPlacement: function(error, element) {
                if (element.attr("name") == "websites3")
                    error.insertAfter("#dvWorkgroup");
                }

            });

            try {
                $("#websites3").msDropDown();
                //alert($('#websites3 option:selected').val());

            } catch (e) {
                alert("Error: " + e.message);
            }

//            $('#websites3').change(function() {
//                $("#t").val($(this).val());
//                if ($("#t").val() == "shopping_cart") {
//                    $('#divImage').show();
//                    $("#calendar").msDropDown();
//                }
//                else { $('#divImage').hide(); }
//                /*
//                '<=t.Value>' = $('option:selected').text();
//                alert('<=t.Value>');
//                alert($(this).val());*/
//            });
        });

        function test() {
            if ($('#calendar option:selected').text() == "Select") {
                $("#v").val($('#calendar option:selected').text());
                //alert('Please select one');
                //return false;
            }
            else {
                $("#v").val($('#calendar option:selected').text());
                //alert($('#websites3 option:selected').text());
                //alert($('#calendar option:selected').text());
                return true;
            }

        }

       
    </script>

    <form id="form1" runat="server">
    <%-- <asp:ScriptManager ID="src" runat="server">
    </asp:ScriptManager>--%>
    <%-- <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>--%>
    <div>
        <%--onchange="alert($('#websites3 option:selected').text());"--%>
        <%--onchange="alert($('#websites3 option:selected').val());"--%>
        <select name="websites3" id="websites3" style="width: 200px;">
            <%-- <%for (int i = 0; i < 5; i++)
              { 
            %>
            <option value="calendar<%=i%>" selected="selected" title="icons/icon_calendar.gif">Calendar<%=i%></option>  
            <%} %>--%>
            
            <option value="0" selected="selected">Select</option>
            <option value="calendar" title="../icons/icon_calendar.gif">Calendar</option>
            <option value="shopping_cart" title="../icons/icon_cart.gif">Shopping Cart</option>
            <option value="cd" title="../icons/icon_cd.gif">CD</option>
            <option value="email" title="../icons/icon_email.gif">Email</option>
            <option value="faq" title="../icons/icon_faq.gif">FAQ</option>
            <option value="games" title="../icons/icon_games.gif">Games</option>
            <option value="music" title="../icons/icon_music.gif">Music</option>
            <option value="phone" title="../icons/icon_phone.gif">Phone</option>
            <option value="graph" title="../icons/icon_sales.gif">Graph</option>
            <option value="secured" title="../icons/icon_secure.gif">Secured</option>
            <option value="video" title="../icons/icon_video.gif">Video</option>
        </select>
        <div id="dvWorkgroup"></div>
        <div id="divImage" style="display: none;">
            <%--<select name="calendar" id="calendar" style="width: 200px;">
                <option value="select" selected="selected">Select</option>
                <%
                    //for (int i = 0; i < 5; i++)
                    // {
                    status s = new status();
                    s.SOperation = "selectall";
                    DataSet a = s.Status();
                    foreach (DataRow r in a.Tables[0].Rows)
                    {
                    string path = "icons/" + r["sStatusIcon"].ToString();
                    %>
                        <option value="calendar<%=r["iStatusid"]%>" title="<%=path%>"><%=r["sStatusName"].ToString()%></option>
                    <%} %>
                <%//}
                %>
            </select>--%>
            <select name="calendar" id="calendar" style="width: 200px;">
                <option value="select" selected="selected">Select</option>
                <%
                    //for (int i = 0; i < 5; i++)
                    // {

                    PriorityDA s = new PriorityDA();
                    PriorityBE sBe = new PriorityBE();
                    sBe.SOperation = "selectall";

                    DataSet a = s.Priority(sBe);
                    foreach (DataRow r in a.Tables[0].Rows)
                    {
                        string path = "../icons/" + r["sPriorityIcon"].ToString();
                %>
                <option value="calendar<%=r["iPriorityId"]%>" title="<%=path%>">
                    <%=r["sPriorityName"].ToString()%></option>
                <%} %>
                <%
                %>
            </select>
        </div>
        <asp:Button ID="btnChk" Text="Submit" OnClientClick="javascript:test();" OnClick="btnChk_Click"
            runat="server" />
        <input type="hidden" id="t" value="" runat="server" />
        <input type="hidden" id="v" value="" runat="server" />
    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
