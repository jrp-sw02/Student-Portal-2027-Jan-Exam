<%@ Page EnableTheming="false"  Language="C#" AutoEventWireup="true" CodeFile="OnlinePayment.aspx.cs" Inherits="OnlinePayment" Debug = "false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1"  method="post" action="<%Response.Write(requestURL);%>">
    <input type="hidden" id="msg" name="msg" value="<%Response.Write(msg);%>" />
    <script language="javascript" type="text/javascript">
        document.forms["form1"].submit();
    </script>
    </form>
</body>
</html>
