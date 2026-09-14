<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="ExamCentreListReport.aspx.cs" Inherits="HO_Rpt_ExamCentreListReport" Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
        function validateform() {
            if (!isBlank("<%=FileUpload1.ClientID %>", "Upload File"))
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
 Examcentre List Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
 <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to MS Access" ToolTip="Export to mdb file"
        ID="imgAccess" ImageUrl="~/images/imgaccess.jpg" runat="server" 
        OnClick="imgAccess_Click" Visible="False" />
    <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" />
    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClientClick="return validateform();"
        OnClick="btnUpload_Click" Visible="False" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
      Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
</asp:Content>

