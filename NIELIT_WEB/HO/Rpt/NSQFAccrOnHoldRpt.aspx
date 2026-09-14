<%@ Page Title="NSQF Accreditation OnHold Report" Language="C#" MasterPageFile="~/MasterPages/Report.master" AutoEventWireup="true"
    CodeFile="NSQFAccrOnHoldRpt.aspx.cs" Inherits="NSQFAccrOnHoldRpt" %>

<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../../Script/GlobalFunction.js"></script>
    <script language="javascript" type="text/javascript">
        function printwindow() {
            window.print();
            return false;
        }
        function validateform() {
           
            if (!isBlankDate("<%=txtPaymentFromDate.ClientID %>", " From Date", "dd-MMM-yyyy"))
                return false;
           
            return true;
        }
    </script>
    <style type="text/css">
        .custom {
            font-family: Courier;
            color: red;
            font-size: 20px;
        }
    </style>
</asp:Content>


<asp:content id="Content3" contentplaceholderid="cpButtons" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:ImageButton OnClientClick="return printwindow();" ClientIDMode="Static" AlternateText="Print"
        ToolTip="Print" ID="imPrint" ImageUrl="~/images/print.gif" runat="server"  visible="false"/>&nbsp;
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export" ToolTip="Export to excel file"
        ID="ibExport" ImageUrl="~/images/Export_Exl.jpg" runat="server" OnClick="ibExport_Click" visible="false" />
    <asp:ImageButton ClientIDMode="Static" AlternateText="Export to PDF" ToolTip="Export to pdf file"
        ID="imgPDF" ImageUrl="~/images/pdf.jpg" runat="server" 
        OnClick="imgPDF_Click" Visible="false" height="30%" width="15%" />
  
</asp:content>

<asp:content id="Content4" contentplaceholderid="cpSubHeader" runat="Server">
    <asp:Label ID="LblRptSubHeader" runat="server"></asp:Label>
    <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr id="dc2" runat="server" visible="true">
            <td align="left" valign="top" >
                                <asp:Label ID="Label1" runat="server" Text="As On Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </td>
           
        </tr>
        <tr id="dc1" runat="server" visible="true">
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
             <asp:TextBox ID="txtPaymentFromDate" runat="server" MaxLength="11" onpaste="return false;" Width="200px" ReadOnly="false" Enabled="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtPaymentFromDate">
                                </asp:CalendarExtender>                                
                                <img id="img2" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                            </ContentTemplate>                           
                        </asp:UpdatePanel>
                </td>

        </tr>        
        
                    </table>
     <div style="text-align: right; margin-top: 10px">
         <asp:Button ID="btnReportChoice" runat="server" Text="Show Report " OnClientClick="return Validate();"
                    OnClick="btnReportChoice_Click"  /> &nbsp;&nbsp;&nbsp;          
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"  />
            </div>        
</asp:content>
<asp:content id="Content2" contentplaceholderid="cpReportHeader" runat="Server">
    NSQF Accreditation OnHold Report</asp:content>
<asp:content id="Content5" contentplaceholderid="cpReportDate" runat="server">
    Report Date:
    <%=DateTime.Now.ToString("dd-MMM-yyyy") %>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cpReportData" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate> 
    <asp:Label ID="lblheading" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="NSQF Accreditation OnHold Report" Visible="false"></asp:Label>     
     <asp:Label ID="lblheadingCandDetails" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="NSQF Accreditation OnHold Report" Visible="false"></asp:Label><br /><br />
     <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
    
    <div id="divReportData" runat="server" style="width: 100%;">
    </div>
           <asp:HiddenField ID="hddbtnR" runat="server" />
                                 </ContentTemplate></asp:UpdatePanel>
                           <div id="divGrid" runat="server" visible="true">
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Label Width="100%" EnableTheming="false" CssClass="error" ID="lblerrorStatistics" Visible="false"
        runat="server"></asp:Label>
                <br />              
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label5" Visible="false"
                    runat="server"></asp:Label><br />  
                            </ContentTemplate>
        </asp:UpdatePanel>
    </div>       
</asp:content>