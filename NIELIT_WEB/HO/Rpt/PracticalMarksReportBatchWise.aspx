<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true" CodeFile="PracticalMarksReportBatchWise.aspx.cs" Inherits="HO_Rpt_PracticalMarksReportBatchWise"  %>

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

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true" >  
</asp:ScriptManager>
    Online Practical Exam Marks Entry CentreWise  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpButtons" Runat="Server">
    <%--<asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server" />&nbsp;--%>
   <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server"  OnClick="imgPDF_Click"
        Visible="True" height="25%" width="11%" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpSubHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpReportDate" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cpReportData" Runat="Server">

    <div>

        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td colspan="2">
                    <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300" > (-1) is marked to represent "candidate was absent" during the exam.</asp:Label>

                     
                </td>
            </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Centre Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Batch &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList ID="ddlCentreCode" runat="server" Height="19px" SkinID="ddl250"
                              Enabled="true"  Width="126px" OnSelectedIndexChanged="ddlCentreCode_SelectedIndexChanged"  AutoPostBack="true" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlBatch" runat="server" Height="22px" SkinID="ddl250" 
                             AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>                 
                </asp:UpdatePanel>
            </td>
           <%--OnSelectedIndexChanged ="ddlBatch_SelectedIndexChanged"--%> 
        </tr>     
    </table>  

        <table style="width: 1014px; height: 53px;"   >
            <tr>
                <td>

                </td>
            </tr>
            <tr>
                
                <td >
                    <asp:Button  runat="server" Text="View" ID ="btnView" OnClick="btnView_Click" Width="150px" />
                </td>
            </tr>
            <tr>
                
                <td >
                    <asp:Label id ="lblReport" runat ="server" CssClass="error" ForeColor="#FF3300" > </asp:Label>
                </td>
            </tr>
        </table>
    </div>
     <div id ="divReportData" runat ="server" style ="width : 100%">

    </div>
</asp:Content>

