<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VirtualAcademyPaymentStatusReportBatchWiseRpt.aspx.cs" Inherits="HO_Rpt_VirtualAcademyPaymentStatusReportBatchWiseRpt" 
     MasterPageFile="~/MasterPages/Report.master" Debug="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    <asp:Label ID="TopHeading" runat="server" text="Virtual Academy Batch Wise Payment Status Report"></asp:Label>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />&nbsp;&nbsp;

    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True"  />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    &nbsp;
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
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
    <br />
    <div id="divReportData" runat="server" style="width: 100%;">
        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
            runat="server"></asp:Label>
    </div>
</asp:Content>
