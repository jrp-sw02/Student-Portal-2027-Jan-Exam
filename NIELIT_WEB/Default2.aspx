<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfdflv" runat="server" ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="Please Enter a Value"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="Encrypt" runat="server" Text="Encrypt" OnClick="Encrypt_Click" />
            <asp:Button ID="Decrypt" runat="server" Text="Decrypt" OnClick="Decrypt_Click" />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Encrypted Text : "></asp:Label>
            <asp:Label ID="Label2" runat="server" style="color:white" Text="Enter Value"></asp:Label>
        </div>
    </form>
</body>
</html>
