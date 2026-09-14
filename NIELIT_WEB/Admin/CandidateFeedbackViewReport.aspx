<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="CandidateFeedbackViewReport.aspx.cs" Inherits="CandidateFeedbackViewReport" Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script language="javascript" type="text/javascript">
     function printwindow() {
         window.print();
         return false;
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    Feeback/Suggestions By 
    <asp:Label ID="lblUserType" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to exl file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
 <asp:Label ID="lblSessions" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
 Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">
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

