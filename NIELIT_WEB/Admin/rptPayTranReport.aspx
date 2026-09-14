<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="rptPayTranReport.aspx.cs" Inherits="HO_Rpt_PaymentRecieptReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    <asp:Label ID="lblHeading" Text="Payment Transaction Report" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <table>
        <tr>
            
            <td>
                <asp:Label ID="Label1" runat="server" Text="Course Category:" 
                    style="font-weight: 700"></asp:Label>
                <asp:Label ID="lblcategory" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Course Name:" 
                    style="font-weight: 700"></asp:Label>
                <asp:Label ID="lblcourse" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Fee Type:" style="font-weight: 700"></asp:Label>
                <asp:Label ID="lblfeetype" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Date From:" 
                    style="font-weight: 700"></asp:Label>
                <asp:Label ID="lblfromdate" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Date To:" style="font-weight: 700"></asp:Label>
                <asp:Label ID="lbldateto" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
</asp:Content>
