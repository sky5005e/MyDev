<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testpretty.aspx.cs" Inherits="RND_testpretty" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incentex</title>    
    <link rel="stylesheet" href="../CSS/Signup_prettyPhoto.css" type="text/css" media="screen" title="prettyPhoto main stylesheet" charset="utf-8" />
    
    <script src="../JS/JQuery/jquery.js" type="text/javascript" charset="utf-8"></script>		
    <script src="../JS/JQuery/jquery-1.3.2.min.js" type="text/javascript" charset="utf-8"></script>		
    
    <script src="../JS/JQuery/jquery.Signup_prettyPhoto.js" type="text/javascript" charset="utf-8"></script>		
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function() {
            $("a[rel^='prettyPhoto']").prettyPhoto({ theme: 'facebook' });
        });
		</script>
</head>
<body>
<div>
    <a href="../UploadedImages/employeePhoto/employee-photo.gif" rel="prettyPhoto[gallery1]">
    <img src="../UploadedImages/employeePhoto/employee-photo.gif" width="60" height="60" alt="Nice building" /></a>
</div>
</body>
</html>

