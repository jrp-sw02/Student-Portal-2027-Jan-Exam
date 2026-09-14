<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HttpErrorPage.aspx.cs" Inherits="HttpErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color:White;">
    <form id="form1" runat="server">
    <div>
        <h2>
            Error!</h2>
        <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
            <asp:Label ID="innerMessage" runat="server" Font-Bold="true" Font-Size="Large" /><br />
            <pre>
        <asp:Label ID="innerTrace" runat="server" />
      </pre>
        </asp:Panel>
        Error occured during the execution of last request:<br />Please go to home page and try again.
        <asp:Label ID="exMessage" Text="" runat="server" Font-Bold="true" Font-Size="Large" />
        <pre>
      <asp:Label ID="exTrace" runat="server" Visible="false" />
    </pre>
        <br />
        Return to the <a href='../Home.aspx'>Home Page</a>
    </div>
    </form>
</body>
</html>
