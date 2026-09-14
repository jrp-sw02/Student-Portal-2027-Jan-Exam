<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationProcessingReport.aspx.cs"
    Inherits="HO_Rpt_RegistrationProcessingReport" MasterPageFile="~/MasterPages/Report.master" Title="Registration Processing Report"
    Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    Registration Processing Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to txt file"
        ID="ibtext" ImageUrl="~/images/smalltxt.png" runat="server" 
        onclick="ibtext_Click" style="padding-bottom:3px;" visible="false"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
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
</asp:Content>
