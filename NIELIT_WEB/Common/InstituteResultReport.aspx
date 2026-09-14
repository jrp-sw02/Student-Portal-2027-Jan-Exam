<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InstituteResultReport.aspx.cs"
    Inherits="Common_InstituteResultReport" MasterPageFile="~/MasterPages/Report.master" %>


<asp:content id="Content1" contentplaceholderid="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cpReportHeader" runat="Server">
    Institute Result Report
</asp:content>
<asp:content id="Content3" contentplaceholderid="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
</asp:content>
