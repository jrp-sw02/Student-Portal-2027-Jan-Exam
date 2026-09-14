<%@ Page Language="C#" EnableTheming ="false" Debug="false" AutoEventWireup="true" CodeFile="CSCPayment.aspx.cs" Inherits="CSCPayment" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
    
</head>
<body >
    <form id="form1"  method="post" action="">
   <%-- <input type="hidden" name="mtrxid" id="mtrxid" value="<%Response.Write(_mtrxid);%>" />
    <input type="hidden" name="oxitrxid" id="oxitrxid" value="<%Response.Write(_oxitrxid);%>" />
    <input type="hidden" name="amount" id="amount" value="<%Response.Write(_amount);%>" />
    <input type="hidden" name="mid" id="mid" value="<%Response.Write(_mid);%>" />
    <input type="hidden" name="smer" id="smer" value="<%Response.Write(_smer);%>" />
    <input type="hidden" name="mitem" id="mitem" value="<%Response.Write(_mitem);%>" />
    <input type="hidden" name="Othervals" id="Othervals" value="<%Response.Write (_Othervals);%>" />--%>
    <script language="javascript" type="text/javascript">
        document.forms["form1"].submit();
    </script>
    </form>
</body>
</html>
