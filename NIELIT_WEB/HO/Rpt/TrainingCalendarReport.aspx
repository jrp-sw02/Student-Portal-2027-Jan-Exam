<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Report.master" CodeFile="TrainingCalendarReport.aspx.cs" Inherits="HO_Rpt_TrainingCalendarReport" %>

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
    Training Calendar Report Details
</asp:Content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
       
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
   <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server"   OnClick="imgPDF_Click"
        Visible="True" height="25%" width="11%" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="pdf" runat="server">
        <div id="divpdf" class="text-center" style="font-size: small; color: darkslateblue"  runat="server" visible="false">
            <h2>Training Calendar Report Details<br />
            </h2>
        </div>
      
        <div class="row">
            <div class="col-lg-12">
                <center>
                   
                      <asp:Label ID="lblCentreName" runat="server"></asp:Label>
                     <br />
                    <asp:Label ID="lblfromdate" runat="server" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lbltodate" runat="server" Font-Size="Medium"></asp:Label>
                </center>
            </div>

        </div>
        <table width="100%" id="trdata" runat="server">            
            <tr>
                <td colspan="2">
                    <div id="divReportData" runat="server" style="width: 100%;">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label Width="100%" EnableTheming="false" ID="lblCount"
                        runat="server" Text="Total Candidates:"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                        runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>





