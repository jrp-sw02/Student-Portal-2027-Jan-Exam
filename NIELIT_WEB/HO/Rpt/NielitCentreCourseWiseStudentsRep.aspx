<%@ Page Title="NIELIT Center CourseWise Students Details Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
     CodeFile="NielitCentreCourseWiseStudentsRep.aspx.cs" Inherits="NielitCentreCourseWiseStudentsRep" Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.focus();
            window.print();
            return false;
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    NIELIT Center CourseWise Students Details Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print" ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
   
     <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="25%" width="10%" />
       
    &nbsp; <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
    Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
     <asp:Label ID="lbldatefromto" runat="server" Font-Bold="True"></asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
<div id="divReportData" runat="server" style="width:100%;">
</div>
</asp:Content>

