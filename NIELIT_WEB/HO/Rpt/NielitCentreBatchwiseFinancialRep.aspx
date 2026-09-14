<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="NielitCentreBatchwiseFinancialRep.aspx.cs" Inherits="NielitCentreBatchwiseFinancialRep"  Debug="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
<script language="javascript" type="text/javascript"> 
    function printwindow() 
    {
        window.print() ; 
        return false ; 
    } 
</script>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server"> 
    Nielit Centre Batch Wise Financial Report 
</asp:Content>
<asp:Content ID="Content3"  ContentPlaceHolderID="cpButtons" runat="server">
  &nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" height="25%" width="11%" />
   &nbsp; <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
&nbsp;
</asp:Content>
<asp:Content ID="Content5"  ContentPlaceHolderID="cpReportDate" runat="server">
   Report Date:  <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>

<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
            </td>
            <td align="right">
                
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
