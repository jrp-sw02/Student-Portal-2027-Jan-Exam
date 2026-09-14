<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PuraskarApp.master" AutoEventWireup="true" CodeFile="BSBStudentDetailReport.aspx.cs" Inherits="HO_Rpt_BSBStudentDetailReport" %>

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
	font:bold 10px arial;
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
	font:normal 6px arial;
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
	font:normal 6px arial;
	color: #000000;
	line-height: 18px; 
}
/*.sample3 Tr.gdrow1 td
{
	padding-left:3px;
}*/
       </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    
     <asp:Label id="lblhead" Text="BSB Student Detail Report" runat="server"></asp:Label>
    
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="True" Height="22px"  />
    &nbsp; <asp:ImageButton ClientIDMode="Static" AlternateText="Export" 
        ToolTip="Export to exl file" ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" 
        runat="server" onclick="ibExport_Click" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function OpenWindow()
        {

            
            var startDate;
            if (!isDate("<%=txttDateFrom.ClientID %>", "From Date", "dd-MMM-yyyy"))
                return false;
            else
                startDate = document.getElementById('<%=txttDateFrom.ClientID %>').value;
            
            var endDate;
            if (!isDate("<%=txtDateto.ClientID %>", "To Date", "dd-MMM-yyyy"))
                return false;
            else
                endDate = document.getElementById('<%=txtDateto.ClientID %>').value;

            if (!CompareDates(startDate, endDate, "DateFrom should be less then  DateTo", true))
                return false;
        }
         </script>


    <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300"  Visible="false"> </asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
        width="20%">
         <tr>

            <td width="5%">
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Form Submitted From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td width="5%">
                <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Form Submitted To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width:5%;" valign="top">
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="BSB U DISE Code  &lt;b class='mandatory'&gt;&lt;/b&gt;"></asp:Label>
            </td>

        </tr>
        <tr>
            <td width="5%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txttDateFrom" runat="server" OnKeyPress="return false" SkinID="txt210" MaxLength="11" onpaste="return false;"
                            ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdate" TargetControlID="txttDateFrom">
                        </asp:CalendarExtender>
                        <img id="imgdate" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="5%">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txtDateto" runat="server" OnKeyPress="return false" SkinID="txt210" MaxLength="11" onpaste="return false;"
                            ToolTip="Date To" AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtDateto">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../../images/calendaricon.jpg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td width="5%">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                       <%-- <asp:DropDownList ID="ddlSemesterNo" runat="server"  Height="22px" Style="width: 270px;">
                            <asp:ListItem Value="0" Text="-- ALL --"></asp:ListItem>
                        </asp:DropDownList>    --%>   
                        <asp:TextBox ID="txtCode" runat="server" ></asp:TextBox>                
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>
        <tr>
            <td colspan ="3">
                 <div style="text-align: center; margin-top: 10px">
                    <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" OnClick="btnView_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
            </td>
        </tr>
       <tr>
           <td colspan="3" width="5%">
                <div id ="divReportData" runat ="server" style ="width : 5%"></div> 
           </td>
       </tr>
    </table>
     
                
     
            
   
</asp:Content>


