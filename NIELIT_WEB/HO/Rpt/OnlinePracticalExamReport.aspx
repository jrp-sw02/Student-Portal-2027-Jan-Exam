<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="OnlinePracticalExamReport.aspx.cs" Inherits="HO_Rpt_OnlinePracticalExamReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <style type="text/css">
        .error
{
    background-color: #EACFCE; 
    padding-top:4px; 
    padding-bottom:4px; 
    padding-left:4px; 
    color: Red;
    border: 1px solid maroon; 
    font-size: 11pt; 
   
}
       .head1
{
	font:bold 13px arial;
	background-color :  #31597C;
	color :#ffffff;
	line-height: 20px; 
	padding-left:2px;
	
}
/*.sample3 Tr.head1 td
{
	padding-left:3px;
}*/
.gdalternate1
{
	font:normal 12px arial;
	background-color : #E6F0F0;
	color :#000000;
	line-height: 18px; 
}
/*/*.sample3 Tr.gdalternate1 td
{
	padding-left:3px;
}*/
.gdrow1
{
	background-color:#c9d7e2;
	font:normal 12px arial;
	color: #000000;
	line-height: 18px; 
}
/*.sample3 Tr.gdrow1 td
{
	padding-left:3px;
}*/
    </style>
    <script language ="javascript"  type="text/javascript">
        function printwindow() {
            window.focus();
            window.print();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpReportHeader" Runat="Server">
    Online Practical Exam Marks Entry CentreWise  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">

    <div>
        <table style="width: 804px" >
            <tr>
                <td>
                    <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300" > (-1) is marked to represent "candidate was absent" during the exam.</asp:Label>
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Button  runat="server" Text="View" ID ="btnView" OnClick="btnView_Click" Width="150px"/>
                </td>
            </tr>
        </table>
    </div>
     <div id ="divReportData" runat ="server" style ="width : 100%">

    </div>
</asp:Content>

