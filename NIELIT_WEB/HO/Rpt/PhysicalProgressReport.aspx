<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Report.master" CodeFile="PhysicalProgressReport.aspx.cs" Inherits="HO_Rpt_PhysicalProgressReport" %>

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
    Trained On-Campus in Quarter and Year
</asp:Content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" runat="server">
       
    
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;
   <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server"  OnClick="imgPDF_Click"
        Visible="True" height="25%" width="11%" />
     <asp:ImageButton ClientIDMode="Static" AlternateText="Export to Excel" ToolTip="Export to Excel"
        ID="imgXL" ImageUrl="~/images/Export_Exl.jpg" runat="server"  OnClick="imgXL_Click"
        Visible="True" height="25%" width="11%" />
     <asp:ImageButton ClientIDMode="Static" AlternateText="Export to Word" ToolTip="Export to Word"
        ID="ImgDOC" ImageUrl="~/images/ExportWord.jpg" runat="server"  OnClick="imgDOC_Click"
        Visible="True" height="45%" width="21%" />
</asp:Content>



<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" runat="Server">
    <div id="pdf" runat="server">
        <div id="divpdf" class="text-center" style="font-size: small; color: darkslateblue"  runat="server" visible="false">
            <h2>Physical Progress Report<br />
            </h2>
        </div>
      
        <div class="row">
            <div class="col-lg-12">
                <center>
                   
                      <asp:Label ID="lblCentreName" runat="server"></asp:Label>
                     <br />
                                    
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
                    <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                        runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>





