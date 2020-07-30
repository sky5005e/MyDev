<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--hi this is ankit shah--%> <title>Untitled Page</title>

    <script src="JS/jQuery/jquery-1.3.2.min.js" type="text/javascript" language="javascript"></script>

    <script src="JS/jQuery/actionmessage.js" type="text/javascript" language="javascript"></script>

    <script src="JS/jQuery/jquery.validate.js" type="text/javascript" language="javascript"></script>

</head>
<body style="background-color: Black;">

    <script type="text/javascript" language="javascript">
        $().ready
        (
            function() {
                setTimeout(function()
                {   
                    window.location='index.aspx';
                },
                4000    
               ); 
            }
         ); 
    </script>

    <form id="form1" runat="server">
        <div style="height: 250px;">
            &nbsp;
        </div>
        <div style="color: White; font-weight: bold;" align="Center">
            <blink>
            Authentication in progress
        </blink>
        </div>
        <div style="width: 100%; border: dotted 1px;" align="center">
            <img src="images/ajax-loader.gif" />
        </div>
    </form>
</body>
</html>
