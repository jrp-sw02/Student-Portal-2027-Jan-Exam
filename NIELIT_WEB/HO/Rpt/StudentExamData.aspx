<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="StudentExamData.aspx.cs" Inherits="Rpt_StudentExamData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    Student Exam Data
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="Server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    &nbsp;
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="Server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="LblRptSubHeader1" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="divReportData" runat="server" style="width: 100%;">
        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
            runat="server"></asp:Label>
    </div>
</asp:Content>
