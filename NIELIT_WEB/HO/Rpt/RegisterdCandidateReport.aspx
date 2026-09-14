<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterdCandidateReport.aspx.cs"
    Inherits="HO_Rpt_RegisterdCandidateReport" MasterPageFile="~/MasterPages/Report.master"
    Title="Registered Candidate Report" Debug="false" %>


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
    Registered Candidate Report
</asp:content>
<asp:content id="Content3" contentplaceholderid="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />&nbsp;
</asp:content>
<asp:content id="Content4" contentplaceholderid="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
</asp:content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
            </td>
            <td align="right">
                <asp:Label Width="100%" EnableTheming="false" ID="lblCount" Visible="false"
                    runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="divReportData" runat="server" style="width: 100%;">
                </div>
            </td>
        </tr>
    </table>
</asp:content>
