<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testamit.aspx.cs" Inherits="CAND_testamit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload Excel" OnClick="btnUpload_Click" />
        <br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
