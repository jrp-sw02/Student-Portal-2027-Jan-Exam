<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="ExamTimeTableReport.aspx.cs" Inherits="Admin_ExamTimeTableReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" runat="Server">
    NIELIT DATE SHEET FOR 
    <asp:Label ID="lblCourse" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="Server">
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" runat="Server">
    <asp:Label ID="lblSessions" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" runat="Server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="divReportData" runat="server" style="width: 100%;">
       <table class="sample2" cellpadding="2" cellspacing="0" id="tblShow" runat="server" visible="false">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblbcc" runat="server" SkinID="CaptionLabel" Text="Theory Papers &lt;b class='mandatory'&gt;&lt;/b&gt;"
                        Width="100%"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
     
</asp:Content>
