<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="Online_Users.aspx.cs" Inherits="ReportPgae" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">
    function printwindow() {
        window.print();
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    Online Users on 
    <%=DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") %>
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
   Select User Type: &nbsp;&nbsp;
    <asp:DropDownList Width="250px" ID="ddlUserType" runat="server" 
        AutoPostBack="true" onselectedindexchanged="ddlUserType_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:Button ID="btnRefresh" runat="server" onclick="btnRefresh_Click" 
        Text="Refresh" ToolTip="Click to refresh list" /><asp:Label ID="lblSessionCount"
            runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
    <asp:Label ID="lblerror" runat="server" EnableTheming="false" CssClass="error" Width="99%"
        Visible="false"> </asp:Label>
<div id="divReportData" runat="server" style="width:100%;">
</div>
</asp:Content>

