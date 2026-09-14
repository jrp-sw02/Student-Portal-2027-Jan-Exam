<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="Activation.aspx.cs" Inherits="Activation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    Account Activation Status
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
<div runat="server" style="height:400px;" widht="100%">
    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" runat="server"></asp:Label>
</div>
    <div align="right">
        <asp:HyperLink ID="hlklink" runat="server" NavigateUrl="~/Index.aspx">Go To Login Page</asp:HyperLink>
    </div>

</asp:Content>
