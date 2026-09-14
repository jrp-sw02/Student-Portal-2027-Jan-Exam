<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="ReportPage.aspx.cs" Inherits="ReportPgae" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function printwindow() {
        window.print();
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    Report Header
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    Report Sub Header
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
<div id="divReportData" runat="server" style="width:100%;">
</div>
</asp:Content>

